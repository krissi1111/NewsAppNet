using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services.Interfaces;
using CryptoHelper;
using Microsoft.AspNetCore.Mvc;

namespace NewsAppNet.Services
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;
        IAuthService authService;

        public UserService(
            IUserRepository userRepository,
            IAuthService authService
            )
        {
            this.userRepository = userRepository;
            this.authService = authService;
        }

        public bool VerifyInput(User user)
        {
            var username = userRepository.GetSingle(u => u.Username == user.Username);
            if (username == null) return false;
            if (!VerifyPassword(user.Password, username.Password)) return false;
            return true;
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public ActionResult<UserAuthData> Login(User user)
        {
            var currentUser = userRepository.GetSingle(u => u.Username == user.Username);
            if (currentUser == null)
            {
                return new BadRequestObjectResult("Username not found");
            }
            if (!VerifyPassword(user.Password, currentUser.Password))
            {
                return new BadRequestObjectResult("Password incorrect");
            }
            return authService.GetUserAuthData(currentUser);
        }

        public ActionResult<UserAuthData> Register(User user)
        {
            var usernameExists = userRepository.UsernameExists(user.Username);
            if (usernameExists == true)
            {
                return new BadRequestObjectResult("Username already exists");
            }

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

            return authService.GetUserAuthData(userRepository.GetSingle(u => u.Username == user.Username));
        }

        public UserAuthData GetUser(int id)
        {
            var user = userRepository.GetSingle(id);

            return authService.GetUserAuthData(user);
        }
    }
}
