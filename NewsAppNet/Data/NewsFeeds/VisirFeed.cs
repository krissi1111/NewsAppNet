using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
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
