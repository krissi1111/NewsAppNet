using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IUserService
    {
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        bool VerifyPasswordLogin(User loginUser);
        ServiceResponse<UserAuthData> Login(string username, string password);
        ServiceResponse<UserAuthData> LoginToken(int userId);
        ServiceResponse<UserAuthData> Register(User user);
        User? GetUser(int userId);
        User? GetUser(string username);
        UserAuthData GetUserAuthData(User user);
        UserAuthData GetUserAuthData(int userId);
        void AddUser(User user);
        ServiceResponse<UserView> DeleteUser(int userDeleteId, int userRequestId);
        ServiceResponse<UserView> RestoreUser(int userRestoreId, int userRequestId);
    }
}
