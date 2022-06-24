using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface ICommentReplyRepository<T> : IEntityBaseRepository<T> 
        where T : class, ICommentReply, new()
    {
        Task <IEnumerable<T>> GetMany(int newsId);
        Task <IEnumerable<T>> GetUserComments(int userId);
    }
}
