using Microsoft.EntityFrameworkCore;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services;
using CryptoHelper;
using NewsAppNet.Data.NewsFeeds.Feeds;

namespace NewsAppNet.Data
{
    public class NewsAppDbContext : DbContext
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public NewsAppDbContext(DbContextOptions<NewsAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderNewsItem(modelBuilder);
            ModelBuilderUser(modelBuilder);
            ModelBuilderComment(modelBuilder);
            ModelBuilderReply(modelBuilder);
            ModelBuilderFavorite(modelBuilder);
        }

        void ModelBuilderNewsItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.ToTable("NewsItems");

                NewsFeedList feedList = new();
                List<NewsItem> newsItems = new List<NewsItem>();

                foreach (NewsFeedBase newsFeed in feedList.FeedList)
                {
                    newsItems.AddRange(newsFeed.GetNewsItems());
                }

                int id = 1;
                newsItems.ForEach(newsItem =>
                {
                    newsItem.Id = id++;
                });

                entity.HasData(newsItems);
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
                entity.HasData(
                    new User
                    {
                        Id = 2,
                        Username = "kalli",
                        FirstName = "Karl",
                        LastName = "Arnarsson",
                        Password = Crypto.HashPassword("passi"),
                        UserType = "User"
                    });
                entity.HasData(
                    new User
                    {
                        Id = 3,
                        Username = "jonas",
                        FirstName = "Jónas",
                        LastName = "Þórsson",
                        Password = Crypto.HashPassword("passi"),
                        UserType = "User"
                    });
                entity.HasData(
                    new User
                    {
                        Id = 4,
                        Username = "Sigga",
                        FirstName = "Sigrún",
                        LastName = "Jónsdóttir",
                        Password = Crypto.HashPassword("passi"),
                        UserType = "User"
                    });
                entity.HasData(
                    new User
                    {
                        Id = 5,
                        Username = "örn",
                        FirstName = "Örn",
                        LastName = "Atlason",
                        Password = Crypto.HashPassword("passi"),
                        UserType = "User"
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

                entity.HasData(
                    new Comment
                    {
                        Id = 1,
                        NewsItemId = 1,
                        UserId = 2,
                        Text = "komment",
                        Date = DateTime.Now,
                    });
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

                entity.HasData(
                    new Reply
                    {
                        Id = 1,
                        NewsItemId = 1,
                        UserId = 3,
                        CommentId = 1,
                        Text = "reply",
                        Date = DateTime.Now,
                    });
            });
        }

        void ModelBuilderFavorite(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(t => new { t.UserId, t.NewsItemId });

                entity
                    .HasOne(t => t.User)
                    .WithMany(t => t.Favorites)
                    .HasForeignKey(t => t.UserId);

                entity
                    .HasOne(t => t.NewsItem)
                    .WithMany(t => t.Favorites)
                    .HasForeignKey(t => t.NewsItemId);

                entity.HasData(
                    new Favorite
                    {
                        Id = 1,
                        UserId = 2,
                        NewsItemId = 1,
                    });
            });
        }
    }
}
