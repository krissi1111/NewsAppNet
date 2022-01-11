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
                viewList.Add(new NewsItemView
                {
                    Title = item.Title,
                    Summary = item.Summary,
                    Link = item.Link,
                    Image = item.Image,
                    Id = item.Id,
                    Date = item.Date,
                    Origin = item.Origin,
                });
            }

            return viewList;
        }

        public NewsItemView GetNews(int Id)
        {
            NewsItem newsItem = newsItemRepository.GetSingle(Id);

            NewsItemView view = new NewsItemView
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Summary = newsItem.Summary,
                Origin = newsItem.Origin,
                Link = newsItem.Link,
                Image = newsItem.Image,
                Date = newsItem.Date,
            };

            return view;
        }

        public void AddNews(NewsItem newsItem)
        {
            newsItemRepository.Add(newsItem);
            newsItemRepository.Commit();
        }

        public void RemoveNews(int Id)
        {
            var news = newsItemRepository.GetSingle(Id);
            newsItemRepository.Delete(news);
            newsItemRepository.Commit();
        }
    }
}
