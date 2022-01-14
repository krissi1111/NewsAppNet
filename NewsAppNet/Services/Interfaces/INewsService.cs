using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface INewsService
    {
        List<NewsItemView> GetNews();
        NewsItemView GetNews(int Id);
        List<NewsItemView> GetNewsSearch(Search search);
        void AddNews(NewsItem newsItem);
        void RemoveNews(int Id);
    }
}
