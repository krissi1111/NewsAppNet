using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Data.NewsFeeds.Feeds;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class NewsService : INewsService
    {
        INewsItemRepository newsItemRepository;
        ICommentReplyService commentReplyService;
        IFavoriteService favoriteService;

        public NewsService(
            INewsItemRepository newsItemRepository,
            ICommentReplyService commentReplyService, 
            IFavoriteService favoriteService
            )
        {
            this.newsItemRepository = newsItemRepository;
            this.commentReplyService = commentReplyService;
            this.favoriteService = favoriteService;
        }

        public List<NewsItemView> GetNews()
        {
            IEnumerable<NewsItem> news = newsItemRepository.GetAll();
            news = news.OrderByDescending(s => s.Date);

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
                viewList.Add(new NewsItemView(item));
            }

            return viewList;
        }

        public List<NewsItemView> AddNews()
        {
            NewsFeedList feedList = new();
            List<NewsItem> newsItems = new List<NewsItem>();
            List<NewsItemView> items = new List<NewsItemView>();

            foreach (NewsFeedBase newsFeed in feedList.FeedList)
            {
                List<NewsItem> newsFeedItems = newsFeed.GetNewsItems();
                foreach(NewsItem item in newsFeedItems)
                {
                    if (newsItemRepository.NewsItemExists(item.Link)) continue;
                    newsItemRepository.Add(item);
                    items.Add(new NewsItemView(item));
                }
            }

            newsItemRepository.Commit();
            return items;
        }

        public void RemoveNews(int Id)
        {
            var news = newsItemRepository.GetSingle(Id);
            newsItemRepository.Delete(news);
            newsItemRepository.Commit();
        }

        public Dictionary<string, List<NewsItemView>> GetPopularNews()
        {
            var newsIdsFav = favoriteService.popularNewsIdFav();
            var newsIdsCom = commentReplyService.popularNewsIdComment();

            var newsItemsFav = newsItemRepository.GetManyOrdered(newsIdsFav);
            var newsItemsCom = newsItemRepository.GetManyOrdered(newsIdsCom);

            var newsViewFav = new List<NewsItemView>();
            foreach (NewsItem item in newsItemsFav)
            {
                newsViewFav.Add(new NewsItemView(item));
            }

            var newsViewCom = new List<NewsItemView>();
            foreach (NewsItem item in newsItemsCom)
            {
                newsViewCom.Add(new NewsItemView(item));
            }

            var dict = new Dictionary<string, List<NewsItemView>>();
            dict.Add("favorites", newsViewFav);
            dict.Add("comments", newsViewCom);

            return dict;
        }
    }
}
