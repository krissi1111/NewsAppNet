using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class RuvFeed : NewsFeedBase
    {
        public RuvFeed()
        {
            Url = "https://www.ruv.is/rss/frettir";
            FeedName = "Ruv";
            ImageDefault = "https://www.ruv.is/sites/all/themes/at_ruv/images/svg/ruv-logo-36.svg?v=1.3";
        }

        public override NewsItemBuilder GetItemBuilder()
        {
            return new RuvItemBuilder();
        }
    }
}
