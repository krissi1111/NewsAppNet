using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Models.DataModels;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class NewsFeedBase
    {
        public string Url { get; set; } = string.Empty;
        public string FeedName { get; set; } = string.Empty;
        public string ImageDefault { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/480px-No_image_available.svg.png";

        public NewsFeedBase(string feedUrl, string feedName, string image)
        {
            Url = feedUrl;
            FeedName = feedName;
            ImageDefault = image;
        }

        public NewsFeedBase(string feedUrl, string feedName)
        {
            Url = feedUrl;
            FeedName = feedName;
        }

        public NewsFeedBase() { }

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
            NewsItemBuilder ItemBuilder = GetItemBuilder();

            List<NewsItem> feedItemList = new();
            foreach (SyndicationItem item in feed.Items)
            {
                NewsItem newsItem = new()
                {
                    NewsFeedId = 1,
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

        public virtual NewsItemBuilder GetItemBuilder()
        {
            return new NewsItemBuilder(ImageDefault);
        }
    }
}
