using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public class DvFeed : NewsFeedBase
    {
        public DvFeed()
        {
            Url = "https://www.dv.is/feed/";
            FeedName = "Dv";
        }

        public override NewsItemBuilder BuildItem(SyndicationItem item)
        {
            return new DvItemBuilder(item);
        }
    }
}
