using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.DataModels
{
    public class Reply : ICommentReply
    {
        public int Id { get; set; }
        public int NewsItemId { get; set; }
        public NewsItem? NewsItem { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public IList<Reply>? Replies { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }
}
