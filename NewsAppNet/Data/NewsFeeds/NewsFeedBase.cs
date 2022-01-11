using NewsAppNet.Models.DataModels;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAppNet.Data.NewsFeeds
{
    public class NewsFeedBase : INewsFeedBase
    {
        public string Url { get; set; }
        public string FeedName { get; set; }

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
            foreach(SyndicationItem item in feed.Items)
            {
                NewsItem entry = GetItem(item);
                feedItemList.Add(entry);
            }
            return feedItemList;
        }

        public virtual NewsItem GetItem(SyndicationItem item)
        {
            NewsItemBuilder itemBuilder = new(item);

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
