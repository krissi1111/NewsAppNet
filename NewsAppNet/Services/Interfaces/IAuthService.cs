using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IAuthService
    {
        UserAuthData GetUserAuthData(User user);
    }
}
