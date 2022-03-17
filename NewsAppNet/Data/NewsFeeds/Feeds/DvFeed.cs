using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class DvFeed : NewsFeedBase
    {
        public DvFeed()
        {
            Url = "https://www.dv.is/feed/";
            FeedName = "Dv";
            ImageDefault = "https://www.dv.is/wp-content/uploads/2018/09/DV.jpg";
        }

        public override NewsItemBuilder GetItemBuilder()
        {
            return new DvItemBuilder();
        }
    }
}
