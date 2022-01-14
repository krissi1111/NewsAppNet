using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface ICommentReplyService
    {
        IEnumerable<Comment> GetComments(int newsId);
        IEnumerable<Reply> GetReplies(int newsId);
        List<CommentView> GetCommentList(int newsId, int userId);
        void AddComment(int newsId, int userId, string commentText);
        void AddReply(int newsId, int userId, int commentId, string commentText);
        /*void UpdateComment(int commentId, int userId, string commentText);
        void DeleteComment(int commentId, int userId);
        void DeleteReply(int replyId, int userId);
        */
    }
}
