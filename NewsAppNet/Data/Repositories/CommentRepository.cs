using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data.Repositories
{
    public class CommentRepository : EntityBaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(NewsAppDbContext context) : base(context) { }

        public async Task<IEnumerable<Comment>> GetMany(int newsId)
        {
            return await GetMany(t => t.NewsItemId == newsId);
        }

        public async Task<IEnumerable<Comment>> GetUserComments(int userId)
        {
            return await GetMany(t => t.UserId == userId);
        }
    }
}
