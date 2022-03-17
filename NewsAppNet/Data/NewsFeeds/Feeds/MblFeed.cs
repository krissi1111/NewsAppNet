using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class MblFeed : NewsFeedBase
    {
        public MblFeed()
        {
            Url = "https://www.mbl.is/feeds/helst/";
            FeedName = "Mbl";
            ImageDefault = "https://www.mbl.is/a/img/haus_new/mbl-dark.svg";
        }

        public override NewsItemBuilder GetItemBuilder()
        {
            return new MblItemBuilder();
        }
    }
}
