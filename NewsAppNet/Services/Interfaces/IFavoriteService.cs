using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IFavoriteService
    {
        List<Favorite> GetUserFavorites(int userId);
        void AddRemoveFavorite(int newsId, int userId);
        void AddFavorite(int newsId, int userId);
        void RemoveFavorite(int newsId, int userId);
        IEnumerable<int> popularNewsIdFav(int amount = 5);
    }
}
