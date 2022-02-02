using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class DvFeed : NewsFeedBase<DvItemBuilder>
    {
        public DvFeed()
        {
            Url = "https://www.dv.is/feed/";
            FeedName = "Dv";
        } 
    }
}
