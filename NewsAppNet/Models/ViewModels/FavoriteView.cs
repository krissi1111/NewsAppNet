using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Models.ViewModels
{
    public class FavoriteView
    {
        public int UserId { get; set; }
        public int NewsItemId { get; set; }

        public FavoriteView(Favorite fav)
        {
            UserId = fav.UserId;
            NewsItemId = fav.NewsItemId;
        }
    }
}
