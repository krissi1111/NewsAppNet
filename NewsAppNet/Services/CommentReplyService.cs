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
        IUserService userService;

        public CommentReplyService(
            ICommentRepository commentRepository, 
            IReplyRepository replyRepository,
            IUserService userService
            )
        {
            this.commentRepository = commentRepository;
            this.replyRepository = replyRepository;
            this.userService = userService;
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
            var replies = GetReplies(newsId).ToList();

            var commentViews = new List<CommentView>();
            foreach (var comment in comments)
            {
                var replyViews = new List<CommentView>();
                foreach(var reply in replies)
                {
                    if (reply.CommentId != comment.Id) continue;
                    var replyView = new CommentView(reply);
                    User userR = userService.GetUser(reply.UserId);
                    replyView.UserFullName = string.Format("{0} {1}", userR.FirstName, userR.LastName);
                    replyViews.Add(replyView);
                }
                
                var commentView = new CommentView(comment);
                User user = userService.GetUser(comment.UserId);
                commentView.UserFullName = string.Format("{0} {1}", user.FirstName, user.LastName);
                commentView.Replies = replyViews;
                commentViews.Add(commentView);
            }

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
