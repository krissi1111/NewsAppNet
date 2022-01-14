using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class ReplyRepository : EntityBaseRepository<Reply>, IReplyRepository
    {
        public ReplyRepository(NewsAppDbContext context) : base(context) { }

        public IEnumerable<Reply> GetMany(int newsId)
        {
            return GetMany(t => t.NewsItemId == newsId);
        }

        public IEnumerable<Reply> GetUserComments(int userId)
        {
            return GetMany(t => t.UserId == userId);
        }
    }
}
