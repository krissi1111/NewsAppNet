using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class VisirFeed : NewsFeedBase
    {
        public VisirFeed()
        {
            Url = "https://www.visir.is/rss/allt";
            FeedName = "Visir";
        }

        public override NewsItemBuilder BuildItem(SyndicationItem item)
        {
            return new VisirItemBuilder(item);
        }
    }
}
