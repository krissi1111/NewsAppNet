using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface INewsItemRepository : IEntityBaseRepository<NewsItem>
    {
        bool NewsItemExists(string ItemLink);
        bool NewsItemExists(int newsId);
        Task<IEnumerable<NewsItem>> GetManyOrdered(IEnumerable<int> ids);
        Task<IEnumerable<NewsItem>> GetNewsIncludeAll();
    }
}
