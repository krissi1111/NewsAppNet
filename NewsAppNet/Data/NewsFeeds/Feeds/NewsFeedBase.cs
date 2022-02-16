using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Models.DataModels;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class NewsFeedBase<T> : INewsFeedBase where T : class, INewsItemBuilder, new()
    {
        public string Url { get; set; } = string.Empty;
        public string FeedName { get; set; } = string.Empty;
        private T ItemBuilder = new T();

        // Attempts to read news feed and returns as SyndicationFeed object
        public SyndicationFeed ReadFeed()
        {
            SyndicationFeed feed = new();
            try
            {
                XmlReader reader = XmlReader.Create(Url);
                feed = SyndicationFeed.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return feed;
        }

        // Transforms all news articles from SyndicationFeed into a 
        // list of NewsItems using the news feeds specific NewsItemBuilder
        public List<NewsItem> GetNewsItems()
        {
            SyndicationFeed feed = ReadFeed();

            List<NewsItem> feedItemList = new();
            foreach (SyndicationItem item in feed.Items)
            {
                NewsItem newsItem = new()
                {
                    Origin = FeedName,
                    Title = ItemBuilder.GetTitle(item),
                    Summary = ItemBuilder.GetSummary(item),
                    Link = ItemBuilder.GetLink(item),
                    Image = ItemBuilder.GetImage(item),
                    Date = ItemBuilder.GetDate(item)
                };

                feedItemList.Add(newsItem);
            }
            return feedItemList;
        }
    }
}
