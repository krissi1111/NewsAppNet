using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class CommentRepository : EntityBaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(NewsAppDbContext context) : base(context) { }

        public IEnumerable<Comment> GetMany(int newsId)
        {
            return GetMany(t => t.NewsItemId == newsId);
        }

        public IEnumerable<Comment> GetUserComments(int userId)
        {
            return GetMany(t => t.UserId == userId);
        }
    }
}
