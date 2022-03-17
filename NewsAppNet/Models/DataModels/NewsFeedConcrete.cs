using NewsAppNet.Data.NewsFeeds.ItemBuilder;

namespace NewsAppNet.Models.DataModels
{
    public static class NewsFeedConcrete
    {
        public static Dictionary<string, NewsItemBuilder> GetFeeds()
        {
            Dictionary<string, NewsItemBuilder> Feeds = new();

            Feeds.Add("Dv", new DvItemBuilder());
            Feeds.Add("Visir", new VisirItemBuilder());
            Feeds.Add("Mbl", new MblItemBuilder());
            Feeds.Add("Ruv", new RuvItemBuilder());

            return Feeds;
        }
    }
}
