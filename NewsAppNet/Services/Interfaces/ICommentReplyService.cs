using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface ICommentReplyService
    {
        IEnumerable<Comment> GetComments(int newsId);
        IEnumerable<Reply> GetReplies(int newsId);
        ServiceResponse<List<CommentView>> GetCommentList(int newsId);
        ServiceResponse<CommentView> AddComment(int newsId, int userId, string commentText);
        ServiceResponse<CommentView> AddReply(int newsId, int userId, int commentId, string commentText);
        ServiceResponse<CommentView> EditComment(int commentId, int userId, string commentText);
        ServiceResponse<CommentView> EditReply(int replyId, int userId, string replyText);
        ServiceResponse<CommentView> DeleteComment(int commentId, int userId);
        ServiceResponse<CommentView> DeleteReply(int replyId, int userId);
    }
}
