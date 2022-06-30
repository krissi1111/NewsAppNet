using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;

namespace NewsAppNet.Services.Interfaces
{
    public interface ICommentReplyService
    {
        Task<ServiceResponse<List<CommentDTO>>> GetComments(int newsId);
        Task<ServiceResponse<CommentDTO>> AddComment(int newsId, int userId, string commentText);
        //Task<ServiceResponse<CommentDTO>> AddReply(int newsId, int userId, int commentId, string commentText);
        Task<ServiceResponse<CommentDTO>> AddReply(int newsId, int userId, int parentId, string commentText);
        Task<ServiceResponse<CommentDTO>> EditComment(int commentId, int userId, string commentText);
        //Task<ServiceResponse<CommentDTO>> EditReply(int replyId, int userId, string replyText);
        Task<ServiceResponse<CommentDTO>> DeleteComment(int commentId, int userId);
        //Task<ServiceResponse<CommentDTO>> DeleteReply(int replyId, int userId);
        Task<ServiceResponse<CommentDTO>> RestoreComment(int commentId, int userId);
        //Task<ServiceResponse<CommentDTO>> RestoreReply(int replyId, int userId);
    }
}
