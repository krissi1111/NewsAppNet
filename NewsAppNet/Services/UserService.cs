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
        readonly IUserRepository userRepository;

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

        public async Task<ServiceResponse<UserAuthData>> Login(string username, string password)
        {
            ServiceResponse<UserAuthData> response = new();

            var user = await GetUser(username);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Incorrect username or password";
                return response;
            }
            else if (user.IsDeleted)
            {
                response.Success = false;
                response.Message = "User account disabled";
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

        public async Task<ServiceResponse<UserAuthData>> LoginToken(int userId)
        {
            ServiceResponse<UserAuthData> response = new();

            var user = await GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            else if (user.IsDeleted)
            {
                response.Success = false;
                response.Message = "User account disabled";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data= GetUserAuthData(user);
                return response;
            }
        }

        public async Task<ServiceResponse<UserAuthData>> Register(User user)
        {
            ServiceResponse<UserAuthData> response = new();

            var usernameExists = await GetUser(user.Username);
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
        public async Task<bool> VerifyPasswordLogin(User loginUser)
        {
            // Get actual user from database
            var user = await userRepository.GetSingle(u => u.Username == loginUser.Username);

            // Compare passwords
            return VerifyPassword(loginUser.Password, user.Password);
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public async Task<User?> GetUser(int userId)
        {
            var user = await userRepository.GetSingle(userId);
            return user;
        }

        public async Task<User?> GetUser(string username)
        {
            var user = await userRepository.GetSingle(u => u.Username == username);
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

        public async Task<UserAuthData> GetUserAuthData(int userId)
        {
            User user = await userRepository.GetSingle(userId);
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

        // Used for deleting users.
        // Parameters are:
        //      UserDeleteId:   id of user that is to be deleted
        //      UserRequestId:  id of user calling the method
        // Only admins are allowed this action
        public async Task<ServiceResponse<UserView>> DeleteUser(int userDeleteId, int userRequestId)
        {
            ServiceResponse<UserView> response = new();

            var requestUser = await GetUser(userRequestId);
            if (requestUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            else if (requestUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }

            var subjectUser = await GetUser(userDeleteId);
            if (subjectUser == null)
            {
                response.Success = false;
                response.Message = string.Format("User {0} not found", userDeleteId);
                return response;
            }
            else if (subjectUser.UserType == "Admin")
            {
                response.Success = false;
                response.Message = "Admin users cannot be deleted";
                return response;
            }
            else if (subjectUser.IsDeleted)
            {
                response.Success = false;
                response.Message = string.Format("User {0} already soft deleted", userDeleteId);
                return response;
            }

            var userView = new UserView(subjectUser);

            userRepository.Delete(subjectUser);
            userRepository.Commit();

            response.Success = true;
            response.Data = userView;

            return response;
        }

        // Used for restoring soft deleted users.
        // Parameters are:
        //      UserRestoreId:  id of user that is to be restored
        //      UserRequestId:  id of user calling the method
        // Only admins are allowed this action
        public async Task<ServiceResponse<UserView>> RestoreUser(int userRestoreId, int userRequestId)
        {
            ServiceResponse<UserView> response = new();

            var requestUser = await GetUser(userRequestId);
            if (requestUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            else if (requestUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }

            var subjectUser = await GetUser(userRestoreId);
            if (subjectUser == null)
            {
                response.Success = false;
                response.Message = string.Format("User {0} not found", userRestoreId);
                return response;
            }
            else if (!subjectUser.IsDeleted)
            {
                response.Success = false;
                response.Message = string.Format("User {0} is not soft deleted", userRestoreId);
                return response;
            }

            subjectUser.IsDeleted = false;
            userRepository.Update(subjectUser);
            userRepository.Commit();

            var userView = new UserView(subjectUser);

            response.Success = true;
            response.Data = userView;

            return response;
        }
    }
}
