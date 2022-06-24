using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class FavoriteService : IFavoriteService
    {
        readonly IFavoriteRepository favoriteRepository;
        readonly INewsItemRepository newsItemRepository;
        readonly IUserService userService;

        public FavoriteService(
            IFavoriteRepository favoriteRepository,
            INewsItemRepository newsItemRepository,
            IUserService userService
            )
        {
            this.favoriteRepository = favoriteRepository;
            this.newsItemRepository = newsItemRepository;
            this.userService = userService;
        }

        public async Task<ServiceResponse<List<FavoriteView>>> GetUserFavorites(int userId)
        {
            ServiceResponse<List<FavoriteView>> response = new();

            var user = userService.GetUser(userId);
            if(user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }

            var fav = await favoriteRepository.GetMany(f => f.UserId == userId);

            List<FavoriteView> favViews = new();
            foreach(var item in fav)
            {
                favViews.Add(new FavoriteView(item));
            }

            response.Success = true;
            response.Data = favViews;
            return response;
        }

        // Adds or removes favorite connection between user and news item.
        // Action taken depends on previous status.
        public ServiceResponse<string> AddRemoveFavorite(int newsId, int userId)
        {
            ServiceResponse<string> response = new();

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }

            if (!newsItemRepository.NewsItemExists(newsId))
            {
                response.Success = false;
                response.Message = "News item not found";
                return response;
            }

            response.Success = true;

            var favExists = favoriteRepository.FavoriteExists(newsId, userId);
            // If news item already set as favorite then remove as favorite
            if (favExists)
            {
                RemoveFavorite(newsId, userId);
                response.Data = string.Format("Removed news item {0} as favorite", newsId);
            }
            // If news item not set as favorite then set as favorite
            else 
            { 
                AddFavorite(newsId, userId);
                response.Data = string.Format("Added news item {0} as favorite", newsId);
            }

            return response;
        }

        public void AddFavorite(int newsId, int userId)
        {
            Favorite fav = new()
            {
                UserId = userId,
                NewsItemId = newsId
            };

            favoriteRepository.Add(fav);
            favoriteRepository.Commit();
        }

        public async void RemoveFavorite(int newsId, int userId)
        {
            Favorite fav = await favoriteRepository.GetSingle(f => f.NewsItemId == newsId && f.UserId == userId);
            
            favoriteRepository.Delete(fav, true);
            favoriteRepository.Commit();
        }
    }
}
