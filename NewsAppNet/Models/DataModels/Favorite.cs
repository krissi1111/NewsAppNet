using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.DataModels
{
    public class Favorite : IEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int NewsItemId { get; set; }
        public NewsItem? NewsItem { get; set; }
    }
}
