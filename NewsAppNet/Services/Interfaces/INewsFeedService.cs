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
        Task<ServiceResponse<List<NewsFeedView>>> GetFeeds(IEnumerable<int>? ids);
        Task<ServiceResponse<List<NewsFeedView>>> GetAll();
        Task<ServiceResponse<List<NewsFeedView>>> GetMany(IEnumerable<int> ids);
        Task<ServiceResponse<NewsFeedView>> GetSingle(int id);
        SyndicationFeed? ReadFeed(string feedUrl);
        List<NewsItem> GetNewsItems(NewsFeedModel newsFeed);
        NewsItemBuilder GetNewsItemBuilder(NewsFeedModel newsFeed);
    }
}
