using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public class RuvFeed : NewsFeedBase
    {
        public RuvFeed()
        {
            Url = "https://www.ruv.is/rss/frettir";
            FeedName = "Ruv";
        }

        public override NewsItemBuilder BuildItem(SyndicationItem item)
        {
            return new RuvItemBuilder(item);
        }
    }
}
