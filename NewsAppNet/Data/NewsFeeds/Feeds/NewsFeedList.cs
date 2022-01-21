namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class NewsFeedList
    {
        public List<NewsFeedBase> FeedList { get; }

        public NewsFeedList()
        {
            FeedList = new List<NewsFeedBase>();

            FeedList.Add(new DvFeed());
            FeedList.Add(new MblFeed());
            FeedList.Add(new RuvFeed());
            FeedList.Add(new VisirFeed());
        }
    }
}
