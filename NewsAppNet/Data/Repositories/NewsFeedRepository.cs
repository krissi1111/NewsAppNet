using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class NewsFeedRepository : EntityBaseRepository<NewsFeedModel>, INewsFeedRepository
    {
        public NewsFeedRepository(NewsAppDbContext context) : base(context) { }

        public bool NewsFeedExists(string feedUrl)
        {
            var newsFeed = GetSingle(feed => feed.FeedUrl == feedUrl);
            return newsFeed != null;
        }

        public bool NewsFeedExists(int id)
        {
            var newsFeed = GetSingle(id);
            return newsFeed != null;
        }
    }
}
