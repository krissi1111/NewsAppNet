using Microsoft.EntityFrameworkCore;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services;
using CryptoHelper;

namespace NewsAppNet.Data
{
    public class NewsAppDbContext : DbContext
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replys { get; set; }

        public NewsAppDbContext(DbContextOptions<NewsAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.ToTable("NewsItems");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasData(
                    new User
                    {
                        Id = 1,
                        Username = "Admin",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Password = Crypto.HashPassword("Admin"),
                        UserType = "Admin"
                    });
            });
            */
            ModelBuilderNewsItem(modelBuilder);
            ModelBuilderUser(modelBuilder);
            ModelBuilderComment(modelBuilder);
            ModelBuilderReply(modelBuilder);
        }

        void ModelBuilderNewsItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.ToTable("NewsItems");
            });
        }

        void ModelBuilderUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasData(
                    new User
                    {
                        Id = 1,
                        Username = "Admin",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Password = Crypto.HashPassword("Admin"),
                        UserType = "Admin"
                    });
            });
        }

        void ModelBuilderComment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");

                entity.HasOne(t => t.User)
                .WithMany(t => t.Comments)
                .HasForeignKey(t => t.UserId);

                entity.HasOne(t => t.NewsItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(t => t.NewsItemId);
            });
        }

        void ModelBuilderReply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reply>(entity =>
            {
                entity.ToTable("Replies");

                entity.HasOne(t => t.User)
                .WithMany(t => t.Replies)
                .HasForeignKey(t => t.UserId);

                entity.HasOne(t => t.NewsItem)
                .WithMany(t => t.Replies)
                .HasForeignKey(t => t.NewsItemId);

                entity.HasOne(t => t.Comment)
                .WithMany(t => t.Replies)
                .HasForeignKey(t => t.CommentId);
            });
        }
    }
}
