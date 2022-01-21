using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsService
    {
        List<NewsItemView> GetNews();
        NewsItemView GetNews(int Id);
        List<NewsItemView> GetNewsSearch(Search search);
        List<NewsItemView> AddNews();
        void RemoveNews(int Id);
        Dictionary<string, List<NewsItemView>> GetPopularNews();
    }
}
