using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Models.ViewModels
{
    public class CommentView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<CommentView> Replies { get; set; }
        public bool IsOwner { get; set; } = false;

        public CommentView(ICommentReply comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            Text = comment.Text;
            Date = comment.Date;
            var replies = new List<CommentView>();
            if (comment.Replies != null) {
                foreach (var item in comment.Replies)
                {
                    replies.Add(new CommentView(item));
                }
            }
            Replies = replies;
        }
    }
}
