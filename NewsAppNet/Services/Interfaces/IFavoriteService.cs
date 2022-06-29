using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<ServiceResponse<string>> AddRemoveFavorite(int newsId, int userId);
        void AddFavorite(int newsId, int userId);
        void RemoveFavorite(int newsId, int userId);
    }
}
