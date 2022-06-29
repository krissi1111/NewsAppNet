using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsAppNet.Models;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;
using NewsAppNet.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsAppNet.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        readonly IMapper mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtBearerTokenSettings> jwtTokenOptions,
            IMapper mapper
            )
        {
            this.userManager = userManager;
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<List<UserDTO>>> GetUsers()
        {
            var users = userManager.Users;

            var userList = mapper.Map<List<UserDTO>>(users);

            return new ServiceResponse<List<UserDTO>> { Success = true, Data = userList };
        }

        public async Task<ServiceResponse<UserDTO>> GetUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return new ServiceResponse<UserDTO> { Success = false, Message = "User not found" };
            }

            var userDTO = mapper.Map<UserDTO>(user);

            return new ServiceResponse<UserDTO> { Success = true, Data = userDTO };
        }

        public async Task<ServiceResponse<UserAuth>> Login(UserLogin userLogin)
        {
            var user = await ValidateLogin(userLogin);
            
            if (user == null)
            {
                return new ServiceResponse<UserAuth> { Success = false, Message = "User not found" };
            }

            var token = GenerateToken(user);
            var userDTO = mapper.Map<UserDTO>(user);

            UserAuth userAuth = new()
            {
                User = userDTO,
                Token = token
            };

            return new ServiceResponse<UserAuth> { Success = true, Data = userAuth };
        }

        public async Task<ServiceResponse<UserAuth>> LoginToken(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return new ServiceResponse<UserAuth> { Success = false, Message = "Token login not successful" };
            }

            var token = GenerateToken(user);
            var userDTO = mapper.Map<UserDTO>(user);

            var userAuth = new UserAuth 
            { 
                User = userDTO, Token = token 
            };

            return new ServiceResponse<UserAuth> 
            { 
                Success = true, 
                Data = userAuth 
            };
        }

        public async Task<ServiceResponse<UserAuth>> Register(UserRegister userRegister)
        {
            var user = mapper.Map<ApplicationUser>(userRegister);

            var result = await userManager.CreateAsync(user, userRegister.Password);

            if (!result.Succeeded)
            {
                return new ServiceResponse<UserAuth>
                {
                    Success = false,
                    Message = "User registration failed"
                };
            }
            var token = GenerateToken(user);
            var userDTO = mapper.Map<UserDTO>(user);

            UserAuth userAuth = new()
            {
                User = userDTO,
                Token = token
            };

            return new ServiceResponse<UserAuth>
            {
                Success = true,
                Data = userAuth
            };
        }

        private async Task<ApplicationUser> ValidateLogin(UserLogin userLogin)
        {
            var user = await userManager.FindByNameAsync(userLogin.UserName);

            if (user != null)
            {
                var result = userManager.PasswordHasher
                    .VerifyHashedPassword(user, user.PasswordHash, userLogin.Password);
                return result == PasswordVerificationResult.Success ? user : null;
            }

            return null;
        }

        private string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString())
                }),

                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
