using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class MblFeed : NewsFeedBase<MblItemBuilder>
    {
        public MblFeed()
        {
            Url = "https://www.mbl.is/feeds/helst/";
            FeedName = "Mbl";
        }
    }
}
