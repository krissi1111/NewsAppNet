using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Data
{
    public class NewsAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<NewsFeedModel> NewsFeeds { get; set; }

        public NewsAppDbContext(DbContextOptions<NewsAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelBuilderNewsFeeds(modelBuilder);
            ModelBuilderNewsItem(modelBuilder);
            //ModelBuilderUser(modelBuilder);
            ModelBuilderComment(modelBuilder);
            //ModelBuilderReply(modelBuilder);
            ModelBuilderFavorite(modelBuilder);
        }

        static void ModelBuilderNewsItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.ToTable("NewsItems");

                entity.HasOne(t => t.NewsFeedModel)
                    .WithMany(t => t.NewsItems)
                    .HasForeignKey(t => t.NewsFeedId);
            });
        }

        /*static void ModelBuilderUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
            });
        }
        */
        static void ModelBuilderComment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");

                entity.HasOne<Comment>()
                    .WithMany(t => t.Replies)
                    .HasForeignKey(t => t.ParentId);

                /*entity.HasOne(t => t.User)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(t => t.UserId);

                entity.HasOne(t => t.NewsItem)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(t => t.NewsItemId);
                */
            });
                
        }
        /*
        static void ModelBuilderReply(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reply>(entity =>
            {
                entity.ToTable("Replies");

                entity.HasOne(t => t.User)
                    .WithMany(t => t.Replies)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.NewsItem)
                    .WithMany(t => t.Replies)
                    .HasForeignKey(t => t.NewsItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Comment)
                    .WithMany(t => t.Replies)
                    .HasForeignKey(t => t.CommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        */

        static void ModelBuilderFavorite(ModelBuilder modelBuilder)
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
            });
        }

        static void ModelBuilderNewsFeeds(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsFeedModel>(entity =>
            {
                entity.ToTable("NewsFeeds");

                entity.HasIndex(u => u.FeedUrl)
                    .IsUnique();
            });
        }
    }
}
