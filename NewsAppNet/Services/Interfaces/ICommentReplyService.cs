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
        void EditComment(int commentId, int userId, string commentText);
        void EditReply(int replyId, int userId, string replyText);
        void DeleteComment(int commentId, int userId);
        void DeleteReply(int replyId, int userId);
        IEnumerable<int> popularNewsIdComment(int amount = 5);
    }
}
