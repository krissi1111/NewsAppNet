using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsService
    {
        bool NewsItemExists(int newsId);
        ServiceResponse<List<NewsItemView>> GetNewsAll();
        ServiceResponse<NewsItemView> GetNews(int Id);
        ServiceResponse<List<NewsItemView>> GetNewsSearch(Search search);
        ServiceResponse<List<NewsItemView>> AddNews(int userId);
        ServiceResponse<NewsItemView> DeleteNews(int newsId, int userId);
        ServiceResponse<Dictionary<string, List<NewsItemView>>> GetPopularNews(int amount = 5);
    }
}
