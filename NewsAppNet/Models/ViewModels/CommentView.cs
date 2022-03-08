using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.ViewModels
{
    public class CommentView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<CommentView> Replies { get; set; } = Enumerable.Empty<CommentView>();

        public CommentView(ICommentReply comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            Text = comment.Text;
            Date = comment.Date;
            IsDeleted = comment.IsDeleted;
        }
    }
}
