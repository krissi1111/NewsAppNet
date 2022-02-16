using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IFavoriteService
    {
        ServiceResponse<List<FavoriteView>> GetUserFavorites(int userId);
        ServiceResponse<string> AddRemoveFavorite(int newsId, int userId);
        void AddFavorite(int newsId, int userId);
        void RemoveFavorite(int newsId, int userId);
    }
}
