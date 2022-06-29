using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsService
    {
        bool NewsItemExists(int newsId);
        Task<ServiceResponse<List<NewsItemDTO>>> GetNewsAll();
        Task<ServiceResponse<NewsItemDTO>> GetNews(int Id);
        Task<ServiceResponse<List<NewsItemDTO>>> GetNewsSearch(Search search);
        Task<ServiceResponse<List<NewsItemDTO>>> AddNews(int userId);
        Task<ServiceResponse<List<NewsItemDTO>>> AddNews(int userId, IEnumerable<int> newsFeedIds);
        Task<ServiceResponse<NewsItemDTO>> DeleteNews(int newsId, int userId);
        Task<ServiceResponse<Dictionary<string, List<NewsItemDTO>>>> GetPopularNews(int amount = 5);
        Task<ServiceResponse<NewsItemDTO>> RestoreNews(int newsId, int userId);
    }
}
