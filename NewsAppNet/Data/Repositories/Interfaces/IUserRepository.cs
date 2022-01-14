using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
        bool UsernameExists(string username);
    }
}
