using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsFeedService
    {
        bool NewsFeedExists(string feedUrl);
        bool NewsFeedExists(int id);
        ServiceResponse<List<NewsFeedView>> GetFeeds(IEnumerable<int>? ids);
        ServiceResponse<List<NewsFeedView>> GetAll();
        ServiceResponse<List<NewsFeedView>> GetMany(IEnumerable<int> ids);
        ServiceResponse<NewsFeedView> GetSingle(int id);
        SyndicationFeed? ReadFeed(string feedUrl);
        List<NewsItem> GetNewsItems(NewsFeedModel newsFeed);
        NewsItemBuilder GetNewsItemBuilder(NewsFeedModel newsFeed);
    }
}
