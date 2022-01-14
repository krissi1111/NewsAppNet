using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class NewsService : INewsService
    {
        INewsItemRepository newsItemRepository;

        public NewsService(
            INewsItemRepository newsItemRepository
            )
        {
            this.newsItemRepository = newsItemRepository;
        }

        public List<NewsItemView> GetNews()
        {
            IEnumerable<NewsItem> news = newsItemRepository.GetAll();

            List<NewsItemView> viewList = new List<NewsItemView>();
            foreach (NewsItem item in news)
            {
                viewList.Add(new NewsItemView(item));
            }

            return viewList;
        }

        public NewsItemView GetNews(int Id)
        {
            NewsItem newsItem = newsItemRepository.GetSingle(Id);

            NewsItemView view = new NewsItemView(newsItem);

            return view;
        }

        public List<NewsItemView> GetNewsSearch(Search search)
        {
            var title = search.Title;
            var summary = search.Summary;
            var dateStart = search.DateStart;
            var dateEnd = search.DateEnd;
            var origin = search.Origin;

            IEnumerable<NewsItem> news = newsItemRepository.GetAll();

            if (!string.IsNullOrEmpty(title))
            {
                var newsT = news.Where(s => s.Title.Contains(title));
                if (!string.IsNullOrEmpty(summary))
                {
                    var newsS = news.Where(s => s.Summary.Contains(summary));
                    news = newsT.Union(newsS);
                }
                else news = newsT;
            }

            if (!string.IsNullOrEmpty(summary) && string.IsNullOrEmpty(title))
            {
                news = news.Where(s => s.Summary.Contains(summary));
            }

            if (!string.IsNullOrEmpty(origin))
            {
                news = news.Where(s => s.Origin.Contains(origin));
            };

            if (!string.IsNullOrEmpty(dateStart))
            {
                news = news.Where(s => s.Date >= DateTime.Parse(dateStart));
            }

            if (!string.IsNullOrEmpty(dateEnd))
            {
                news = news.Where(s => s.Date <= DateTime.Parse(dateEnd));
            }

            news = news.OrderByDescending(s => s.Date);

            List<NewsItemView> viewList = new List<NewsItemView>();
            foreach (NewsItem item in news)
            {
                Console.WriteLine(dateEnd);
                viewList.Add(new NewsItemView(item));
            }

            return viewList;
        }

        public void AddNews(NewsItem newsItem)
        {
            if (!newsItemRepository.NewsItemExists(newsItem.Link)) { 
                newsItemRepository.Add(newsItem);
                newsItemRepository.Commit();
            }
        }

        public void RemoveNews(int Id)
        {
            var news = newsItemRepository.GetSingle(Id);
            newsItemRepository.Delete(news);
            newsItemRepository.Commit();
        }
    }
}
