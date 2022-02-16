using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services.Interfaces;
using CryptoHelper;
using NewsAppNet.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace NewsAppNet.Services
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;

        readonly string jwtSecret;
        readonly int jwtLifespan;

        public UserService(
            IUserRepository userRepository,
            IConfiguration configuration
            )
        {
            this.userRepository = userRepository;
            jwtSecret = configuration.GetValue<string>("JWTSecretKey");
            jwtLifespan = configuration.GetValue<int>("JWTLifespan");
        }

        public ServiceResponse<UserAuthData> Login(string username, string password)
        {
            ServiceResponse<UserAuthData> response = new();

            var user = GetUser(username);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Incorrect username or password";
                return response;
            }

            bool correctPassword = VerifyPassword(password, user.Password);
            if (!correctPassword)
            {
                response.Success = false;
                response.Message = "Incorrect username or password";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data = GetUserAuthData(user);
                return response;
            }
        }

        public ServiceResponse<UserAuthData> LoginToken(int userId)
        {
            ServiceResponse<UserAuthData> response = new();

            var user = GetUser(userId);
            if(user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else
            {
                response.Success = true;
                response.Data= GetUserAuthData(user);
            }

            return response;
        }

        public ServiceResponse<UserAuthData> Register(User user)
        {
            ServiceResponse<UserAuthData> response = new();

            var usernameExists = GetUser(user.Username);
            if (usernameExists != null)
            {
                response.Success = false;
                response.Message = "Username already exists";
                return response;
            }

            try
            {
                AddUser(user);
                response.Success = true;
                response.Data = GetUserAuthData(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("User registration error: {0}", ex.Message);
                response.Success = false;
                response.Message = "User registration failed";
            }

            return response;
        }

        // Compares password from login attempt with actual password
        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
        }

        // Checks if users password provided in login attempt 
        // is the same as the users password stored in database
        public bool VerifyPasswordLogin(User loginUser)
        {
            // Get actual user from database
            var user = userRepository.GetSingle(u => u.Username == loginUser.Username);

            // Compare passwords
            return VerifyPassword(loginUser.Password, user.Password);
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public User? GetUser(int userId)
        {
            var user = userRepository.GetSingle(userId);
            return user;
        }

        public User? GetUser(string username)
        {
            var user = userRepository.GetSingle(u => u.Username == username);
            return user;
        }

        public UserAuthData GetUserAuthData(User user)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(jwtLifespan);

            UserView userView = new(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType)
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new UserAuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToString(),
                User = userView
            };
        }

        public UserAuthData GetUserAuthData(int userId)
        {
            User user = userRepository.GetSingle(userId);
            return GetUserAuthData(user);
        }

        // Used to add user when password hasn't been hashed
        public void AddUser(User user)
        {
            var newUser = new User
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = HashPassword(user.Password),
                UserType = user.UserType
            };
            
            userRepository.Add(newUser);
            userRepository.Commit();
        }
    }
}
