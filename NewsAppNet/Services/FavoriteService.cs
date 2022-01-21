using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class FavoriteService : IFavoriteService
    {
        IFavoriteRepository favoriteRepository;

        public FavoriteService(
            IFavoriteRepository favoriteRepository
            )
        {
            this.favoriteRepository = favoriteRepository;
        }

        public List<Favorite> GetUserFavorites(int userId)
        {
            var fav = favoriteRepository.GetMany(f => f.UserId == userId);
            return fav.ToList();
        }

        public void AddRemoveFavorite(int newsId, int userId)
        {
            if(userId != -1)
            {
                var favExists = favoriteRepository.FavoriteExists(newsId, userId);
                if(favExists) RemoveFavorite(newsId, userId);
                else AddFavorite(newsId, userId);
            }
        }

        public void AddFavorite(int newsId, int userId)
        {
            Favorite fav = new Favorite
            {
                UserId = userId,
                NewsItemId = newsId
            };

            favoriteRepository.Add(fav);
            favoriteRepository.Commit();
        }

        public void RemoveFavorite(int newsId, int userId)
        {
            Favorite fav = favoriteRepository.GetSingle(f => f.NewsItemId == newsId && f.UserId == userId);
            
            favoriteRepository.Delete(fav);
            favoriteRepository.Commit();
        }

        public IEnumerable<int> popularNewsIdFav(int amount = 5)
        {
            var popularFavorites = favoriteRepository.GetAll()
                .GroupBy(t => t.NewsItemId)
                .OrderByDescending(t => t.Count())
                .Select(t => t.Key)
                .Take(amount);

            return popularFavorites;
        }
    }
}
