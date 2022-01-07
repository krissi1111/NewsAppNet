using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public class NewsItemRepository : EntityBaseRepository<NewsItem>, INewsItemRepository
    {
        public NewsItemRepository(NewsAppDbContext context) : base(context) { }
    }
}
