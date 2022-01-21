using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Models.DataModels;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAppNet.Data.NewsFeeds.Feeds
{
    public class NewsFeedBase : INewsFeedBase
    {
        public string Url { get; set; } = string.Empty;
        public string FeedName { get; set; } = string.Empty;

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

        public virtual NewsItemBuilder BuildItem(SyndicationItem item)
        {
            return new NewsItemBuilder(item);
        }

        public NewsItem GetItem(SyndicationItem item)
        {
            NewsItemBuilder itemBuilder = BuildItem(item);

            NewsItem news = new()
            {
                Origin = FeedName,
                Title = itemBuilder.GetTitle(),
                Summary = itemBuilder.GetSummary(),
                Link = itemBuilder.GetLink(),
                Image = itemBuilder.GetImage(),
                Date = itemBuilder.GetDate()
            };

            return news;
        }
    }
}
