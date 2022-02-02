using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class VisirFeed : NewsFeedBase<VisirItemBuilder>
    {
        public VisirFeed()
        {
            Url = "https://www.visir.is/rss/allt";
            FeedName = "Visir";
        }
    }
}
