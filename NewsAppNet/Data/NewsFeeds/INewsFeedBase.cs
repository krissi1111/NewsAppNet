using NewsAppNet.Models.DataModels;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public interface INewsFeedBase
    {
        SyndicationFeed ReadFeed();
        List<NewsItem> GetNewsItems();
        NewsItem GetItem(SyndicationItem item);
    }
}
