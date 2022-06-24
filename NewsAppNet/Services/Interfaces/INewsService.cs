using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsService
    {
        bool NewsItemExists(int newsId);
        Task<ServiceResponse<List<NewsItemView>>> GetNewsAll();
        Task<ServiceResponse<NewsItemView>> GetNews(int Id);
        Task<ServiceResponse<List<NewsItemView>>> GetNewsSearch(Search search);
        Task<ServiceResponse<List<NewsItemView>>> AddNews(int userId);
        Task<ServiceResponse<List<NewsItemView>>> AddNews(int userId, IEnumerable<int> newsFeedIds);
        Task<ServiceResponse<NewsItemView>> DeleteNews(int newsId, int userId);
        Task<ServiceResponse<Dictionary<string, List<NewsItemView>>>> GetPopularNews(int amount = 5);
        Task<ServiceResponse<NewsItemView>> RestoreNews(int newsId, int userId);
    }
}
