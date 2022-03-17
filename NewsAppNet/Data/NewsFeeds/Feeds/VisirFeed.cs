using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class VisirFeed : NewsFeedBase
    {
        public VisirFeed()
        {
            Url = "https://www.visir.is/rss/allt";
            FeedName = "Visir";
            ImageDefault = "https://www.visir.is/static/1.0.553/img/visirhvitblar.png";
        }

        public override NewsItemBuilder GetItemBuilder()
        {
            return new VisirItemBuilder();
        }
    }
}
