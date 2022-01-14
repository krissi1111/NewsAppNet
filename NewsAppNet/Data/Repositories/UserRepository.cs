using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(NewsAppDbContext context) : base(context) { }

        public bool UsernameExists(string username)
        {
            var user = GetSingle(user => user.Username == username);
            return user != null;
        }
    }
}
