using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.DataModels
{
    public class Comment : ICommentReply
    {
        public int Id { get; set; }
        public int NewsItemId { get; set; }
        public NewsItem? NewsItem { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public IList<Reply>? Replies { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
