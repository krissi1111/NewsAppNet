using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IUserService
    {
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        Task<bool> VerifyPasswordLogin(User loginUser);
        Task<ServiceResponse<UserAuthData>> Login(string username, string password);
        Task<ServiceResponse<UserAuthData>> LoginToken(int userId);
        Task<ServiceResponse<UserAuthData>> Register(User user);
        Task<User?> GetUser(int userId);
        Task<User?> GetUser(string username);
        UserAuthData GetUserAuthData(User user);
        Task<UserAuthData> GetUserAuthData(int userId);
        void AddUser(User user);
        Task<ServiceResponse<UserView>> DeleteUser(int userDeleteId, int userRequestId);
        Task<ServiceResponse<UserView>> RestoreUser(int userRestoreId, int userRequestId);
    }
}
