using Microsoft.EntityFrameworkCore;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data
{
    public class NewsAppDbContext : DbContext
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        public NewsAppDbContext(DbContextOptions<NewsAppDbContext> options) : base(options) { }
    }
}
