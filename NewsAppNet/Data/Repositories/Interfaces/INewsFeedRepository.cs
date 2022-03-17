using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface INewsFeedRepository : IEntityBaseRepository<NewsFeedModel>
    {
        bool NewsFeedExists(string feedUrl);
        bool NewsFeedExists(int id);
    }
}
