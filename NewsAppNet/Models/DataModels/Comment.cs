using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.DataModels
{
    public class Comment : IEntityBase
    {
        public int Id { get; set; }
        public int NewsItemId { get; set; }
        public NewsItem? NewsItem { get; set; }
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool TopLevelComment { get; set; } = true;
        public int? ParentId { get; set; }
        public IEnumerable<Comment>? Replies { get; set; }
    }
}
