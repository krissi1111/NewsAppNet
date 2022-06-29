using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;
using NewsAppNet.Models.DTOs;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class NewsService : INewsService
    {
        readonly INewsItemRepository newsItemRepository;
        readonly UserManager<ApplicationUser> userManager;
        readonly INewsFeedService newsFeedService;
        readonly INewsFeedRepository newsFeedRepository;
        readonly IMapper mapper;

        public NewsService(
            INewsItemRepository newsItemRepository,
            UserManager<ApplicationUser> userManager,
            //IUserService userService,
            INewsFeedService newsFeedService,
            INewsFeedRepository newsFeedRepository,
            IMapper mapper
            )
        {
            this.newsItemRepository = newsItemRepository;
            this.userManager = userManager;
            this.newsFeedService = newsFeedService;
            this.newsFeedRepository = newsFeedRepository;
            this.mapper = mapper;
        }

        public bool NewsItemExists(int newsId)
        {
            return newsItemRepository.NewsItemExists(newsId);
        }

        // Gets all news items and returns, ordered by most recent
        public async Task<ServiceResponse<List<NewsItemDTO>>> GetNewsAll()
        {
            ServiceResponse<List<NewsItemDTO>> response = new();

            // Get all news items, ordered by most recent
            //IEnumerable<NewsItem> news = await newsItemRepository.GetAllInclude(n => n.Comments, n => n.Replies);
            var news = await newsItemRepository.GetNewsIncludeAll();
            
            news.OrderByDescending(s => s.Date);
             
            var newsList = mapper.Map<List<NewsItemDTO>>(news);

            response.Data = newsList;
            response.Success = true;
            return response;
        }

        // Get specific news item
        public async Task<ServiceResponse<NewsItemDTO>> GetNews(int Id)
        {
            ServiceResponse<NewsItemDTO> response = new();

            if (!NewsItemExists(Id))
            {
                response.Success = false;
                response.Message = string.Format("No news item with id: {0}", Id.ToString());
                return response;
            }

            NewsItem newsItem = await newsItemRepository.GetSingle(Id);
            if (newsItem.IsDeleted)
            {
                response.Success = false;
                response.Message = string.Format("News item with id: {0} is not available", Id.ToString());
                return response;
            }

            response.Data = mapper.Map<NewsItemDTO>(newsItem);
            response.Success= true;

            return response;
        }

        // Returns news items based on search criteria
        public async Task<ServiceResponse<List<NewsItemDTO>>> GetNewsSearch(Search search)
        {
            ServiceResponse<List<NewsItemDTO>> response = new();

            var title = search.Title;
            var summary = search.Summary;
            var dateStart = search.DateStart;
            var dateEnd = search.DateEnd;
            var newsFeedIds = search.NewsFeedIds;

            if(!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
            {
                var start = DateTime.Parse(dateStart);
                var end = DateTime.Parse(dateEnd);

                if(DateTime.Compare(start, end) > 0)
                {
                    response.Success = false;
                    response.Message = "End date cannot be earlier than start date";
                    return response;
                }
            }

            IEnumerable<NewsItem> news = await newsItemRepository.GetNewsIncludeAll();

            if (newsFeedIds != null)
            {
                news = news.Where(s => newsFeedIds.Contains(s.NewsFeedId));
            }

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

            if (!string.IsNullOrEmpty(dateStart))
            {
                news = news.Where(s => s.Date >= DateTime.Parse(dateStart));
            }

            if (!string.IsNullOrEmpty(dateEnd))
            {
                news = news.Where(s => s.Date <= DateTime.Parse(dateEnd));
            }

            news = news.OrderByDescending(s => s.Date);
            
            var newsViews = mapper.Map<List<NewsItemDTO>>(news);

            response.Success = true;
            response.Data = newsViews;

            return response;
        }

        // Adds new news items from all news feeds
        public async Task<ServiceResponse<List<NewsItemDTO>>> AddNews(int userId)
        {
            ServiceResponse<List<NewsItemDTO>> response = new();

            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
            }*/

            var newsFeeds = await newsFeedRepository.GetAll();
            var newsFeedIds = newsFeeds.Select(feed => feed.Id);
            return await AddNews(userId, newsFeedIds);
        }

        public async Task<ServiceResponse<List<NewsItemDTO>>> AddNews(int userId, IEnumerable<int> newsFeedIds)
        {
            ServiceResponse<List<NewsItemDTO>> response = new();

            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
            }*/

            List<NewsItemDTO> itemDTOs = new();
            IEnumerable<NewsFeedModel> newsFeeds;

            newsFeeds = await newsFeedRepository.GetMany(newsFeedIds);

            foreach(NewsFeedModel feed in newsFeeds)
            {
                List<NewsItem> items = newsFeedService.GetNewsItems(feed);
                foreach (NewsItem item in items)
                {
                    if (newsItemRepository.NewsItemExists(item.Link)) continue;
                    newsItemRepository.Add(item);
                }
                itemDTOs.AddRange(mapper.Map<List<NewsItemDTO>>(items));
            }
            newsItemRepository.Commit();

            response.Success = true;
            response.Data = itemDTOs;
            return response;
        }

        // Marks a single news item as deleted (soft delete)
        // Only for admins
        public async Task<ServiceResponse<NewsItemDTO>> DeleteNews(int newsId, int userId)
        {
            ServiceResponse<NewsItemDTO> response = new();

            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }*/

            var news = await newsItemRepository.GetSingle(newsId);
            if(news == null)
            {
                response.Success = false;
                response.Message = "News item not found";
                return response;
            }
            var newsItem = mapper.Map<NewsItemDTO>(news);

            newsItemRepository.Delete(news);
            newsItemRepository.Commit();

            response.Success = true;
            response.Data = newsItem;

            return response;
        }

        // Returns most popular news items based on number of comments or favorites
        public async Task<ServiceResponse<Dictionary<string, List<NewsItemDTO>>>> GetPopularNews(int amount = 5)
        {
            ServiceResponse<Dictionary<string, List<NewsItemDTO>>> response = new();

            //var newsItemsFav = await newsItemRepository.GetManyOrdered(popularFavorites);
            //var newsItemsFav = await newsItemRepository.GetAllInclude(n => n.Favorites);
            //var newsItemsCom = await newsItemRepository.GetAllInclude(n => n.Comments, n => n.Replies);
            //var newsItems = await newsItemRepository.GetNewsIncludeAll();
            var newsItems = await newsItemRepository.GetNewsIncludeAll();

            var newsIF = newsItems.OrderByDescending(n => n.Favorites.Count()).Take(5);
            var newsIC = newsItems.OrderByDescending(n => n.Comments.Count()).Take(5);

            var newsDTOFav = mapper.Map<List<NewsItemDTO>>(newsIF);
            var newsDTOCom = mapper.Map<List<NewsItemDTO>>(newsIC);

            var dict = new Dictionary<string, List<NewsItemDTO>>
            {
                { "favorites", newsDTOFav },
                { "comments", newsDTOCom }
            };

            response.Success = true;
            response.Data = dict;
            
            return response;
        }

        // Used for restoring soft deleted news items
        public async Task<ServiceResponse<NewsItemDTO>> RestoreNews(int newsId, int userId)
        {
            ServiceResponse<NewsItemDTO> response = new();

            var news = await newsItemRepository.GetSingle(newsId);
            if (news == null)
            {
                response.Success = false;
                response.Message = "News item not found";
                return response;
            }
            else if (!news.IsDeleted)
            {
                response.Success = false;
                response.Message = string.Format("News item {0} is not soft deleted", newsId);
                return response;
            }

            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }*/

            news.IsDeleted = false;
            newsItemRepository.Update(news);
            newsItemRepository.Commit();

            var newsItem = mapper.Map<NewsItemDTO>(news);

            response.Success = true;
            response.Data = newsItem;

            return response;
        }
    }
}
