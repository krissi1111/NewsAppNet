using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<NewsItem>> GetManyOrdered(IEnumerable<int> ids)
        {
            var newsItems = new List<NewsItem>();
            foreach (var id in ids)
            {
                newsItems.Add(await GetSingle(id));
            }

            return newsItems;
        }

        public async Task<IEnumerable<NewsItem>> GetNewsIncludeAll()
        {
            var news = GetQueryable();
            news = news.Include(n => n.Comments).ThenInclude(c => c.User);
            news = news.Include(n => n.NewsFeedModel);
            news = news.Include(n => n.Favorites);

            foreach (var item in news)
            {
                item.Comments = item.Comments.Where(c => c.TopLevelComment).ToList();
            }

            return await news.ToListAsync();
        }
    }
}
