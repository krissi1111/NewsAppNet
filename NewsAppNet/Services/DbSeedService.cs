using CryptoHelper;
using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data.NewsFeeds.Feeds;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class DbSeedService : IDbSeedService 
    {
        DbContext _dbContext;

        public DbSeedService(DbContext dbContext) { 
            _dbContext = dbContext;
        }

        // Seeds the database with data if it hasn't already been seeded
        // It is assumed that data has already been seeded if user with username "admin" exists
        public void SeedDb()
        {
            // Check if "admin" exists
            var admin = _dbContext.Set<User>().Where(user => user.Username == "admin").FirstOrDefault();
            // Seed if "admin" doesn't exist
            if (admin == null)
            {
                SeedNewsItem();
                SeedUser();
                SeedComment();
                SeedReply();
                SeedFavorite();
            }
        }

        // Adds news items from all news feeds
        void SeedNewsItem()
        {
            NewsFeedList feedList = new();

            foreach (INewsFeedBase newsFeed in feedList.FeedList)
            {
                List<NewsItem> newsFeedItems = newsFeed.GetNewsItems();
                foreach (NewsItem item in newsFeedItems)
                {
                    _dbContext.Set<NewsItem>().Add(item);
                }
            }

            _dbContext.SaveChanges();
        }

        // Adds special admin user and four general users
        void SeedUser()
        {
            List<User> UserList = new();

            UserList.Add(new User
            {
                Username = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Password = Crypto.HashPassword("admin"),
                UserType = "Admin"
            });
            UserList.Add(new User
            {
                Username = "kalli",
                FirstName = "Karl",
                LastName = "Arnarsson",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });
            UserList.Add(new User
            {
                Username = "jonas",
                FirstName = "Jónas",
                LastName = "Þórsson",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });
            UserList.Add(new User
            {
                Username = "Sigga",
                FirstName = "Sigrún",
                LastName = "Jónsdóttir",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });
            UserList.Add(new User
            {
                Username = "örn",
                FirstName = "Örn",
                LastName = "Atlason",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });

            foreach (var user in UserList)
            {
                _dbContext.Set<User>().Add(user);
            }

            _dbContext.SaveChanges();
        }

        // Tries to add comments if users and news items exist
        void SeedComment()
        {
            // Get up to 5 users if exist
            List<User> users = _dbContext.Set<User>().Take(5).ToList();
            // Get up to 5 news items if exist
            List<NewsItem> newsItems = _dbContext.Set<NewsItem>().Take(5).ToList();

            // Skips if no news items
            if (newsItems.Count > 0)
            {
                var rand = new Random();
                // Add one comment for each user to random news item
                // Skips if no users
                foreach (var user in users)
                {
                    // Picks random news item
                    var news = newsItems[rand.Next(newsItems.Count())];
                    _dbContext.Set<Comment>().Add(new Comment
                    {
                        NewsItemId = news.Id,
                        UserId = user.Id,
                        Text = "Komment"
                    });
                }

                _dbContext.SaveChanges();
            }
        }

        void SeedReply()
        {
            // Get up to 5 users if exist
            List<User> users = _dbContext.Set<User>().Take(5).ToList();
            // Get up to 5 comments if exist
            List<Comment> comments = _dbContext.Set<Comment>().Take(5).ToList();

            // Skips if no comments
            if (comments.Count > 0)
            {
                var rand = new Random();
                // Add one reply for each user to random comment
                // Skips if no users
                foreach (var user in users)
                {
                    // Picks random comment
                    var comment = comments[rand.Next(comments.Count())];
                    _dbContext.Set<Reply>().Add(new Reply
                    {
                        CommentId = comment.Id,
                        NewsItemId = comment.NewsItemId,
                        UserId = user.Id,
                        Text = "Reply"
                    });
                }

                _dbContext.SaveChanges();
            }
        }

        void SeedFavorite()
        {
            // Get up to 5 users if exist
            List<User> users = _dbContext.Set<User>().Take(5).ToList();
            // Get up to 5 news items if exist
            List<NewsItem> newsItems = _dbContext.Set<NewsItem>().Take(5).ToList();

            // Skips if no news items
            if (newsItems.Count > 0)
            {
                var rand = new Random();
                // Add one favorite for each user to random news item
                // Skips if no users
                foreach (var user in users)
                {
                    // Picks random news item
                    var news = newsItems[rand.Next(newsItems.Count())];
                    _dbContext.Set<Favorite>().Add(new Favorite
                    {
                        NewsItemId = news.Id,
                        UserId = user.Id,
                    });
                }

                _dbContext.SaveChanges();
            }
        }
    }
}
