using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class MblFeed : NewsFeedBase
    {
        public MblFeed()
        {
            Url = "https://www.mbl.is/feeds/helst/";
            FeedName = "Mbl";
        }

        public override NewsItemBuilder BuildItem(SyndicationItem item)
        {
            return new MblItemBuilder(item);
        }
    }
}
