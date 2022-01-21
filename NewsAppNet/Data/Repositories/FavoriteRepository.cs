using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class FavoriteRepository : EntityBaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(NewsAppDbContext context) : base(context) { }

        public bool FavoriteExists(int newsId, int userId)
        {
            var fav = GetSingle(f => f.UserId == userId && f.NewsItemId == newsId);
            return fav != null;
        }
    }
}
