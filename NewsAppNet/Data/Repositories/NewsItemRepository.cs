using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class NewsItemRepository : EntityBaseRepository<NewsItem>, INewsItemRepository
    {
        public NewsItemRepository(NewsAppDbContext context) : base(context) { }

        public bool NewsItemExists(string itemLink)
        {
            var news = GetSingle(news => news.Link == itemLink);
            return news != null;
        }

        public bool NewsItemExists(int newsId)
        {
            var news = GetSingle(newsId);
            return news != null;
        }

        public IEnumerable<NewsItem> GetManyOrdered(IEnumerable<int> ids)
        {
            var newsItems = new List<NewsItem>();
            foreach (var id in ids)
            {
                newsItems.Add(GetSingle(id));
            }

            return newsItems;
        }
    }
}
