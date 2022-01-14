using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories.Interfaces
{
    public interface ICommentReplyRepository<T> : IEntityBaseRepository<T> 
        where T : class, ICommentReply, new()
    {
        IEnumerable<T> GetMany(int newsId);
        IEnumerable<T> GetUserComments(int userId);
    }
}
