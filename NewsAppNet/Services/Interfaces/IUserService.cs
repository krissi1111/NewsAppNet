using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;

namespace NewsAppNet.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserDTO>>> GetUsers();
        Task<ServiceResponse<UserDTO>> GetUser(string userName);
        Task<ServiceResponse<UserAuth>> Login(UserLogin userLogin);
        Task<ServiceResponse<UserAuth>> LoginToken(string userName);
        Task<ServiceResponse<UserAuth>> Register(UserRegister userRegister);
    }
}
