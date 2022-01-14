using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class CommentReplyService : ICommentReplyService
    {
        ICommentRepository commentRepository;
        IReplyRepository replyRepository;

        public CommentReplyService(
            ICommentRepository commentRepository, 
            IReplyRepository replyRepository
            )
        {
            this.commentRepository = commentRepository;
            this.replyRepository = replyRepository;
        }

        public IEnumerable<Comment> GetComments(int newsId)
        {
            return commentRepository.GetMany(t => t.NewsItemId == newsId);
        }

        public IEnumerable<Reply> GetReplies(int newsId)
        {
            return replyRepository.GetMany(t => t.NewsItemId == newsId);
        }

        public List<CommentView> GetCommentList(int newsId, int userId)
        {
            var comments = GetComments(newsId).ToList();

            var commentViews = new List<CommentView>();
            foreach (var comment in comments) commentViews.Add(new CommentView(comment));

            return commentViews;
        }

        public void AddComment(int newsId, int userId, string commentText)
        {
            Comment comment = new Comment
            {
                NewsItemId = newsId,
                UserId = userId,
                Text = commentText
            };

            commentRepository.Add(comment);
            commentRepository.Commit();
        }

        public void AddReply(int newsId, int userId, int commentId, string commentText)
        {
            Reply reply = new Reply
            {
                NewsItemId = newsId,
                UserId = userId,
                CommentId = commentId,
                Text = commentText
            };

            replyRepository.Add(reply);
            replyRepository.Commit();
        }
    }
}
