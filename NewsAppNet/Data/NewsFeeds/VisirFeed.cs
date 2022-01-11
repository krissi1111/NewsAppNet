using NewsAppNet.Models.DataModels;
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

        public override NewsItem GetItem(SyndicationItem item)
        {
            return base.GetItem(item);
        }
    }
}
