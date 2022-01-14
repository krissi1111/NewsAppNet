using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IUserService
    {
        bool VerifyInput(User user);
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        ActionResult<UserAuthData> Login(User user);
        ActionResult<UserAuthData> Register(User user);
        User GetUser(int id);
    }
}
