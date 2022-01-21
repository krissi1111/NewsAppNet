using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface IFavoriteRepository : IEntityBaseRepository<Favorite>
    {
        bool FavoriteExists(int newsId, int userId);
    }
}
