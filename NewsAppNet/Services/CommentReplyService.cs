using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class CommentReplyService : ICommentReplyService
    {
        ICommentRepository commentRepository;
        IReplyRepository replyRepository;
        IUserService userService;
        INewsItemRepository newsItemRepository;

        public CommentReplyService(
            ICommentRepository commentRepository, 
            IReplyRepository replyRepository,
            IUserService userService,
            INewsItemRepository newsItemRepository
            )
        {
            this.commentRepository = commentRepository;
            this.replyRepository = replyRepository;
            this.userService = userService;
            this.newsItemRepository = newsItemRepository;
        }

        // Returns all comments for given news item.
        public IEnumerable<Comment> GetComments(int newsId)
        {
            return commentRepository.GetMany(t => t.NewsItemId == newsId);
        }

        // Returns all replies for given news item.
        public IEnumerable<Reply> GetReplies(int newsId)
        {
            return replyRepository.GetMany(t => t.NewsItemId == newsId);
        }

        // Returns all comments and replies for given news item.
        // Replies are grouped under parent comment.
        public ServiceResponse<List<CommentView>> GetCommentList(int newsId)
        {
            ServiceResponse<List<CommentView>> response = new();

            if (!newsItemRepository.NewsItemExists(newsId))
            {
                response.Success = false;
                response.Message = "News item not found";
                return response;
            }

            // Get all comments and replies
            var comments = GetComments(newsId).ToList();
            var replies = GetReplies(newsId).ToList();

            // Go through each comment
            var commentViews = new List<CommentView>();
            foreach (var comment in comments)
            {
                // Find replies belonging to comment
                var replyViews = new List<CommentView>();
                foreach(var reply in replies)
                {
                    if (reply.CommentId != comment.Id) continue;
                    var replyView = new CommentView(reply);
                    var userR = userService.GetUser(reply.UserId);
                    string fullNameR = "(Deleted)";
                    if (userR != null)
                    {
                        fullNameR = string.Format("{0} {1}", userR.FirstName, userR.LastName);
                    }
                    replyView.UserFullName = fullNameR;
                    replyViews.Add(replyView);
                }
                
                var commentView = new CommentView(comment);
                var user = userService.GetUser(comment.UserId);
                string fullName = "(Deleted)";
                if (user != null)
                {
                    fullName = string.Format("{0} {1}", user.FirstName, user.LastName);
                }
                commentView.UserFullName = fullName;
                commentView.Replies = replyViews;
                commentViews.Add(commentView);
            }

            response.Success = true;
            response.Data = commentViews;

            return response;
        }

        public ServiceResponse<CommentView> AddComment(int newsId, int userId, string commentText)
        {
            ServiceResponse<CommentView> response = new();

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            Comment comment = new Comment
            {
                NewsItemId = newsId,
                UserId = userId,
                Text = commentText
            };

            commentRepository.Add(comment);
            commentRepository.Commit();

            response.Success = true;
            response.Data = new CommentView(comment);

            return response;
        }

        public ServiceResponse<CommentView> AddReply(int newsId, int userId, int commentId, string commentText)
        {
            ServiceResponse<CommentView> response = new();

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            Reply reply = new Reply
            {
                NewsItemId = newsId,
                UserId = userId,
                CommentId = commentId,
                Text = commentText
            };

            replyRepository.Add(reply);
            replyRepository.Commit();

            response.Success = true;
            response.Data = new CommentView(reply);

            return response;
        }

        // Used for editing already existing comments.
        // Users can only edit their own comments.
        // Admin can edit any comment.
        public ServiceResponse<CommentView> EditComment(int commentId, int userId, string commentText)
        {
            ServiceResponse<CommentView> response = new();

            var comment = commentRepository.GetSingle(commentId);
            if (comment == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", commentId);
                return response;
            }

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only edit their own comments
            // Admin can edit any comment
            else if(!(user.Id == comment.UserId || user.UserType == "Admin"))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            comment.Text = commentText;
            commentRepository.Update(comment);
            commentRepository.Commit();

            response.Success = true;
            response.Data = new CommentView(comment);

            return response;
        }

        // Used for editing already existing replies.
        // Users can only edit their own replies.
        // Admin can edit any reply.
        public ServiceResponse<CommentView> EditReply(int replyId, int userId, string commentText)
        {
            ServiceResponse<CommentView> response = new();

            var reply = replyRepository.GetSingle(replyId);
            if (reply == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", replyId);
                return response;
            }

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only edit their own comments
            // Admin can edit any comment
            else if (!(user.Id == reply.UserId || user.UserType == "Admin"))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            reply.Text = commentText;
            replyRepository.Update(reply);
            replyRepository.Commit();

            response.Success = true;
            response.Data = new CommentView(reply);

            return response;
        }

        // Used for deleting comments.
        // Users can only delete their own comments.
        // Admin can delete any comment.
        public ServiceResponse<CommentView> DeleteComment(int commentId, int userId)
        {
            ServiceResponse<CommentView> response = new();

            var comment = commentRepository.GetSingle(commentId);
            if (comment == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", commentId);
                return response;
            }

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only delete their own comments
            // Admin can delete any comment
            else if (!(user.Id == comment.UserId || user.UserType == "Admin"))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            // Need to manually delete replies associated with this comment
            // because of foreign key relationships
            var replies = GetReplies(comment.NewsItemId).ToList();
            var replyViews = new List<CommentView>();
            foreach (Reply reply in replies)
            {
                // "replies" includes all replies for the comments news item
                // Only deletes replies associated with comment in question
                if (reply.CommentId == commentId)
                {
                    replyViews.Add(new CommentView(reply));
                    replyRepository.Delete(reply);
                }
            }
            replyRepository.Commit();

            var commentView = new CommentView(comment);
            commentView.Replies = replyViews;
            response.Data = commentView;

            commentRepository.Delete(comment);
            commentRepository.Commit();

            response.Success = true;

            return response;
        }

        // Used for deleting replies.
        // Users can only delete their own replies.
        // Admin can delete any reply.
        public ServiceResponse<CommentView> DeleteReply(int replyId, int userId)
        {
            ServiceResponse<CommentView> response = new();

            var reply = replyRepository.GetSingle(replyId);
            if (reply == null)
            {
                response.Success = false;
                response.Message = string.Format("Reply {0} not found", replyId);
                return response;
            }

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only delete their own replies
            // Admin can delete any reply
            else if (!(user.Id == reply.UserId || user.UserType == "Admin"))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            response.Data = new CommentView(reply);

            replyRepository.Delete(reply);
            replyRepository.Commit();

            response.Success = true;

            return response;
        }
    }
}
