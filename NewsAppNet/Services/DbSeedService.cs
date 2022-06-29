using CryptoHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services.Interfaces;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAppNet.Services
{
    public class DbSeedService : IDbSeedService 
    {
        readonly DbContext _dbContext;
        readonly UserManager<ApplicationUser> userManager;
        readonly RoleManager<IdentityRole<int>> roleManager;

        public DbSeedService(
            DbContext dbContext, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager
            ) 
        {
            this._dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // Seeds the database with data if it hasn't already been seeded
        // It is assumed that data has already been seeded if user with username "admin" exists
        public async Task SeedDb()
        {
            // Check if "admin" exists
            //var admin = _dbContext.Set<User>().Where(user => user.Username == "admin").FirstOrDefault();
            var admin = await userManager.FindByNameAsync("admin");
            // Seed if "admin" doesn't exist
            if (admin == null)
            {
                SeedNewsFeeds();
                SeedNewsItem();
                await SeedUser();
                SeedComment();
                SeedFavorite();
            }
        }

        void SeedNewsFeeds()
        {
            List<NewsFeedModel> feedList = new();

            feedList.Add(new NewsFeedModel
            {
                FeedName = "Mbl",
                FeedUrl = "https://www.mbl.is/feeds/helst/",
                ImageDefault = "https://www.mbl.is/a/img/haus_new/mbl-dark.svg",
                IsConcrete = true,
            });
            feedList.Add(new NewsFeedModel
            {
                FeedName = "Visir",
                FeedUrl = "https://www.visir.is/rss/allt",
                ImageDefault = "https://www.visir.is/static/1.0.553/img/visirhvitblar.png",
                IsConcrete = true,
            });
            feedList.Add(new NewsFeedModel
            {
                FeedName = "Dv",
                FeedUrl = "https://www.dv.is/feed/",
                ImageDefault = "https://www.dv.is/wp-content/uploads/2018/09/DV.jpg",
                IsConcrete = true,
            });
            feedList.Add(new NewsFeedModel
            {
                FeedName = "Ruv",
                FeedUrl = "https://www.ruv.is/rss/frettir",
                ImageDefault = "https://www.ruv.is/sites/all/themes/at_ruv/images/svg/ruv-logo-36.svg?v=1.3",
                IsConcrete = true,
            });
            feedList.Add(new NewsFeedModel
            {
                FeedName = "CNN",
                FeedUrl = "http://rss.cnn.com/rss/edition_world.rss",
                ImageDefault = "https://logos-world.net/wp-content/uploads/2020/11/CNN-Logo.png",
                IsConcrete = false,
            });

            foreach (var feed in feedList)
            {
                _dbContext.Set<NewsFeedModel>().Add(feed);
            }

            _dbContext.SaveChanges();
        }

        // Adds news items from all news feeds
        void SeedNewsItem()
        {
            // Check if news feeds have been seeded.
            // If not call SeedNewsFeed method
            var newsFeeds = _dbContext.Set<NewsFeedModel>().AsEnumerable();
            if (newsFeeds == null) 
            { 
                SeedNewsFeeds();
                newsFeeds = _dbContext.Set<NewsFeedModel>().AsEnumerable();
            }

            newsFeeds.ToList();

            List<NewsItem> feedItemList = new();
            foreach (var newsFeed in newsFeeds)
            {
                SyndicationFeed feed = ReadFeed(newsFeed.FeedUrl);
                NewsItemBuilder ItemBuilder;

                var concreteFeeds = NewsFeedConcrete.GetFeeds();
                if (!concreteFeeds.ContainsKey(newsFeed.FeedName)) ItemBuilder = new NewsItemBuilder(newsFeed.ImageDefault);
                else ItemBuilder = concreteFeeds[newsFeed.FeedName];

                foreach (SyndicationItem item in feed.Items)
                {
                    NewsItem newsItem = new()
                    {
                        NewsFeedId = newsFeed.Id,
                        Title = ItemBuilder.GetTitle(item),
                        Summary = ItemBuilder.GetSummary(item),
                        Link = ItemBuilder.GetLink(item),
                        Image = ItemBuilder.GetImage(item),
                        Date = ItemBuilder.GetDate(item)
                    };
                    feedItemList.Add(newsItem);
                }
            }

            _dbContext.Set<NewsItem>().AddRange(feedItemList);
            _dbContext.SaveChanges();
            /*
            NewsFeedList feedList = new();

            foreach (NewsFeedBase newsFeed in feedList.FeedList)
            {
                List<NewsItem> newsFeedItems = newsFeed.GetNewsItems();
                foreach (NewsItem item in newsFeedItems)
                {
                    _dbContext.Set<NewsItem>().Add(item);
                }
            }

            _dbContext.SaveChanges();
            */
        }

        public SyndicationFeed ReadFeed(string feedUrl)
        {
            SyndicationFeed feed = new();
            try
            {
                XmlReader reader = XmlReader.Create(feedUrl);
                feed = SyndicationFeed.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return feed;
        }

        public NewsItemBuilder GetNewsItemBuilder(NewsFeedModel newsFeed)
        {
            // Non-concrete feeds always use the general news item builder
            if (!newsFeed.IsConcrete) return new NewsItemBuilder(newsFeed.ImageDefault);

            // Fetch dictionary object that ties concrete feeds
            // to their specific concrete news item builders
            var feeds = NewsFeedConcrete.GetFeeds();

            // If dedicated news item builder for the concrete feed
            // is not found, returns the general builder
            if (!feeds.ContainsKey(newsFeed.FeedName)) return new NewsItemBuilder(newsFeed.ImageDefault);
            else return feeds[newsFeed.FeedName];
        }


        async Task SeedUser()
        {
            var superAdminRole = new IdentityRole<int>() { Name = "superAdmin" };
            var adminRole = new IdentityRole<int>() { Name = "admin" };

            await roleManager.CreateAsync(superAdminRole);
            await roleManager.CreateAsync(adminRole);

            var admin = new ApplicationUser()
            {
                UserName = "admin",
                FirstName = "Super",
                LastName = "Admin"
            };

            var password = "Superadmin1";
            await userManager.CreateAsync(admin, password);

            var user = await userManager.FindByNameAsync("admin");
            await userManager.AddToRolesAsync(user, new[] { "superAdmin", "admin" });

            List<ApplicationUser> userList = new();

            userList.Add(new ApplicationUser
            {
                UserName = "kalli",
                FirstName = "Karl",
                LastName = "Arnarsson",
            });
            userList.Add(new ApplicationUser
            {
                UserName = "jonas",
                FirstName = "Jónas",
                LastName = "Þórsson",
            });
            userList.Add(new ApplicationUser
            {
                UserName = "Sigga",
                FirstName = "Sigrún",
                LastName = "Jónsdóttir",
            });
            userList.Add(new ApplicationUser
            {
                UserName = "örn",
                FirstName = "Örn",
                LastName = "Atlason",
            });

            var userPassword = "Passipassi1";
            foreach (var u in userList)
            {
                await userManager.CreateAsync(u, userPassword);
            }
        }
        // Adds special admin user and four general users
        /*void SeedUser()
        {
            List<User> userList = new();

            userList.Add(new User
            {
                Username = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Password = Crypto.HashPassword("admin"),
                UserType = "Admin"
            });
            userList.Add(new User
            {
                Username = "kalli",
                FirstName = "Karl",
                LastName = "Arnarsson",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });
            userList.Add(new User
            {
                Username = "jonas",
                FirstName = "Jónas",
                LastName = "Þórsson",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });
            userList.Add(new User
            {
                Username = "Sigga",
                FirstName = "Sigrún",
                LastName = "Jónsdóttir",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });
            userList.Add(new User
            {
                Username = "örn",
                FirstName = "Örn",
                LastName = "Atlason",
                Password = Crypto.HashPassword("passi"),
                UserType = "User"
            });

            foreach (var user in userList)
            {
                _dbContext.Set<User>().Add(user);
            }

            _dbContext.SaveChanges();
        }
        */
        // Tries to add comments if users and news items exist
        void SeedComment()
        {
            // Get up to 5 users if exist
            //List<User> users = _dbContext.Set<User>().Take(5).ToList();

            List<ApplicationUser> users = userManager.Users.ToList();
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
                    var news = newsItems[rand.Next(newsItems.Count)];
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

        /*void SeedReply()
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
                    var comment = comments[rand.Next(comments.Count)];
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
        */
        void SeedFavorite()
        {
            // Get up to 5 users if exist
            //List<User> users = _dbContext.Set<User>().Take(5).ToList();
            List <ApplicationUser> users = userManager.Users.ToList();
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
                    var news = newsItems[rand.Next(newsItems.Count)];
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
