using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Data.NewsFeeds.Feeds;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class NewsFeedList
    {
        public List<INewsFeedBase> FeedList { get; } = new();

        public NewsFeedList()
        {
            FeedList.Add(new DvFeed());
            FeedList.Add(new MblFeed());
            FeedList.Add(new RuvFeed());
            FeedList.Add(new VisirFeed());
        }
    }
}
