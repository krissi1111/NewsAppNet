using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsFeedService
    {
        bool NewsFeedExists(string feedUrl);
        bool NewsFeedExists(int id);
        Task<ServiceResponse<List<NewsFeedDTO>>> GetFeeds(IEnumerable<int>? ids);
        Task<ServiceResponse<List<NewsFeedDTO>>> GetAll();
        Task<ServiceResponse<List<NewsFeedDTO>>> GetMany(IEnumerable<int> ids);
        Task<ServiceResponse<NewsFeedDTO>> GetSingle(int id);
        SyndicationFeed? ReadFeed(string feedUrl);
        List<NewsItem> GetNewsItems(NewsFeedModel newsFeed);
        NewsItemBuilder GetNewsItemBuilder(NewsFeedModel newsFeed);
    }
}
