using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Services.Interfaces
{
    public interface ICommentReplyService
    {
        Task<IEnumerable<Comment>> GetComments(int newsId);
        Task<IEnumerable<Reply>> GetReplies(int newsId);
        Task<ServiceResponse<List<CommentView>>> GetCommentList(int newsId, int userId);
        Task<ServiceResponse<CommentView>> AddComment(int newsId, int userId, string commentText);
        Task<ServiceResponse<CommentView>> AddReply(int newsId, int userId, int commentId, string commentText);
        Task<ServiceResponse<CommentView>> EditComment(int commentId, int userId, string commentText);
        Task<ServiceResponse<CommentView>> EditReply(int replyId, int userId, string replyText);
        Task<ServiceResponse<CommentView>> DeleteComment(int commentId, int userId);
        Task<ServiceResponse<CommentView>> DeleteReply(int replyId, int userId);
        Task<ServiceResponse<CommentView>> RestoreComment(int commentId, int userId);
        Task<ServiceResponse<CommentView>> RestoreReply(int replyId, int userId);
    }
}
