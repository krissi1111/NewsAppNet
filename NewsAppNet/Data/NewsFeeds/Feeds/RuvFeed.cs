using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class RuvFeed : NewsFeedBase<RuvItemBuilder>
    {
        public RuvFeed()
        {
            Url = "https://www.ruv.is/rss/frettir";
            FeedName = "Ruv";
        }
    }
}
