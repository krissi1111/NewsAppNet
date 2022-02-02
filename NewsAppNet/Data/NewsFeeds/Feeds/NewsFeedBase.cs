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

        public SyndicationFeed ReadFeed()
        {
            XmlReader reader = XmlReader.Create(Url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            return feed;
        }

        public List<NewsItem> GetNewsItems()
        {
            SyndicationFeed feed = ReadFeed();

            List<NewsItem> feedItemList = new();
            foreach (SyndicationItem item in feed.Items)
            {
                NewsItem entry = GetItem(item);
                feedItemList.Add(entry);
            }
            return feedItemList;
        }

        public NewsItem GetItem(SyndicationItem item)
        {
            NewsItem news = new()
            {
                Origin = FeedName,
                Title = ItemBuilder.GetTitle(item),
                Summary = ItemBuilder.GetSummary(item),
                Link = ItemBuilder.GetLink(item),
                Image = ItemBuilder.GetImage(item),
                Date = ItemBuilder.GetDate(item)
            };

            return news;
        }
    }
}
