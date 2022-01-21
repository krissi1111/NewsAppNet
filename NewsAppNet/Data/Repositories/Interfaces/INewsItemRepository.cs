using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface INewsItemRepository : IEntityBaseRepository<NewsItem>
    {
        bool NewsItemExists(string ItemLink);
        IEnumerable<NewsItem> GetManyOrdered(IEnumerable<int> ids);
    }
}
