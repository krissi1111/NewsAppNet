using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class NewsItemRepository : EntityBaseRepository<NewsItem>, INewsItemRepository
    {
        public NewsItemRepository(NewsAppDbContext context) : base(context) { }

        public bool NewsItemExists(string ItemLink)
        {
            var news = GetSingle(news => news.Link == ItemLink);
            return news != null;
        }
    }
}
