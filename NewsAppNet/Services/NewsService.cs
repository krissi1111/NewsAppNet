using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Data.NewsFeeds.Feeds;
using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class NewsService : INewsService
    {
        INewsItemRepository newsItemRepository;
        ICommentRepository commentRepository;
        IFavoriteRepository favoriteRepository;
        ICommentReplyService commentReplyService;
        IFavoriteService favoriteService;
        IUserService userService;

        public NewsService(
            INewsItemRepository newsItemRepository,
            ICommentRepository commentRepository,
            IFavoriteRepository favoriteRepository,
            ICommentReplyService commentReplyService, 
            IFavoriteService favoriteService,
            IUserService userService
            )
        {
            this.newsItemRepository = newsItemRepository;
            this.commentRepository = commentRepository;
            this.favoriteRepository = favoriteRepository;
            this.commentReplyService = commentReplyService;
            this.favoriteService = favoriteService;
            this.userService = userService;
        }

        public bool NewsItemExists(int newsId)
        {
            return newsItemRepository.NewsItemExists(newsId);
        }

        // Gets all news items and returns, ordered by most recent
        public ServiceResponse<List<NewsItemView>> GetNewsAll()
        {
            ServiceResponse<List<NewsItemView>> response = new();

            // Get all news items, ordered by most recent
            IEnumerable<NewsItem> news = newsItemRepository.GetAll()
                .OrderByDescending(s => s.Date);

            List<NewsItemView> newsViews = new List<NewsItemView>();
            foreach (NewsItem item in news)
            {
                newsViews.Add(new NewsItemView(item));
            }

            response.Data = newsViews;
            response.Success = true;
            return response;
        }

        // Get specific news item
        public ServiceResponse<NewsItemView> GetNews(int Id)
        {
            ServiceResponse<NewsItemView> response = new();

            if (!NewsItemExists(Id))
            {
                response.Success = false;
                response.Message = string.Format("No news item with id: {0}", Id.ToString());
                return response;
            }

            NewsItem newsItem = newsItemRepository.GetSingle(Id);
            NewsItemView newsItemView = new NewsItemView(newsItem);

            response.Data = newsItemView;
            response.Success= true;

            return response;
        }

        // Returns news items based on search criteria
        public ServiceResponse<List<NewsItemView>> GetNewsSearch(Search search)
        {
            ServiceResponse<List<NewsItemView>> response = new();

            var title = search.Title;
            var summary = search.Summary;
            var dateStart = search.DateStart;
            var dateEnd = search.DateEnd;
            var origin = search.Origin;

            if(!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
            {
                var start = DateTime.Parse(dateStart);
                var end = DateTime.Parse(dateEnd);

                if(DateTime.Compare(start, end) > 0)
                {
                    response.Success = false;
                    response.Message = "End date is earlier than start date";
                    return response;
                }
            }

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
            }

            if (!string.IsNullOrEmpty(dateStart))
            {
                news = news.Where(s => s.Date >= DateTime.Parse(dateStart));
            }

            if (!string.IsNullOrEmpty(dateEnd))
            {
                news = news.Where(s => s.Date <= DateTime.Parse(dateEnd));
            }

            news = news.OrderByDescending(s => s.Date);

            List<NewsItemView> newsViews = new List<NewsItemView>();
            foreach (NewsItem item in news)
            {
                newsViews.Add(new NewsItemView(item));
            }

            response.Success = true;
            response.Data = newsViews;

            return response;
        }

        // Adds new news items from all news feeds
        public ServiceResponse<List<NewsItemView>> AddNews(int userId)
        {
            ServiceResponse<List<NewsItemView>> response = new();

            var currentUser = userService.GetUser(userId);
            if(currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
            }
            else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
            }

            List<NewsItemView> items = new List<NewsItemView>();

            NewsFeedList feedList = new();

            foreach (INewsFeedBase newsFeed in feedList.FeedList)
            {
                List<NewsItem> newsFeedItems = newsFeed.GetNewsItems();
                foreach (NewsItem item in newsFeedItems)
                {
                    if (newsItemRepository.NewsItemExists(item.Link)) continue;
                    newsItemRepository.Add(item);
                    items.Add(new NewsItemView(item));
                }
            }
            newsItemRepository.Commit();

            response.Data = items;
            response.Success = true;
            return response;
        }

        // Deletes a single news item
        // Only for admins
        public ServiceResponse<NewsItemView> DeleteNews(int newsId, int userId)
        {
            ServiceResponse<NewsItemView> response = new();

            var currentUser = userService.GetUser(userId);
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }

            var news = newsItemRepository.GetSingle(newsId);
            if(news == null)
            {
                response.Success = false;
                response.Message = "News item not found";
                return response;
            }
            var newsItemView = new NewsItemView(news);

            // Need to manually delete comments and replies 
            // because of foreign key relationships
            var comments = commentReplyService.GetComments(newsId);
            foreach (Comment comment in comments)
            {
                commentReplyService.DeleteComment(comment.Id, userId);
            }

            newsItemRepository.Delete(news);
            newsItemRepository.Commit();

            response.Success = true;
            response.Data = newsItemView;

            return response;
        }

        // Returns most popular news items based on number of comments or favorites
        public ServiceResponse<Dictionary<string, List<NewsItemView>>> GetPopularNews(int amount = 5)
        {
            ServiceResponse<Dictionary<string, List<NewsItemView>>> response = new();

            // Get most favorited news items
            var popularFavorites = favoriteRepository.GetAll()
                .GroupBy(t => t.NewsItemId)
                .OrderByDescending(t => t.Count())
                .Select(t => t.Key)
                .Take(amount);

            // Get most commented news items
            var popularComments = commentRepository.GetAll()
                .GroupBy(t => t.NewsItemId)
                .OrderByDescending(t => t.Count())
                .Select(t => t.Key)
                .Take(amount);

            var newsItemsFav = newsItemRepository.GetManyOrdered(popularFavorites);
            var newsItemsCom = newsItemRepository.GetManyOrdered(popularComments);

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

            response.Success = true;
            response.Data = dict;
            
            return response;
        }
    }
}
