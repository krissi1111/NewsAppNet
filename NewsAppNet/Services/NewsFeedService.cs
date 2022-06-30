using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsAppNet.Data.NewsFeeds.ItemBuilder;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;
using NewsAppNet.Services.Interfaces;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsAppNet.Services
{
    public class NewsFeedService : INewsFeedService
    {
        readonly INewsFeedRepository newsFeedRepository;
        readonly UserManager<ApplicationUser> userManager;
        //readonly IUserService userService;
        readonly IMapper mapper;

        public NewsFeedService(
            INewsFeedRepository newsFeedRepository,
            UserManager<ApplicationUser> userManager,
            //IUserService userService,
            IMapper mapper
            )
        {
            this.newsFeedRepository = newsFeedRepository;
            this.userManager = userManager;
            //this.userService = userService;
            this.mapper = mapper;
        }

        // Checks if RSS feed url is already in use
        public bool NewsFeedExists(string feedUrl)
        {
            return newsFeedRepository.NewsFeedExists(feedUrl);
        }

        public bool NewsFeedExists(int id)
        {
            return newsFeedRepository.NewsFeedExists(id);
        }

        public async Task<ServiceResponse<List<NewsFeedDTO>>> GetFeeds(IEnumerable<int>? ids)
        {
            // If news feed id array not supplied or is empty, returns all feeds
            if (ids == null || !ids.Any())
            {
                return await GetAll();
            }
            else return await GetMany(ids);
        }

        // Gets all news feeds
        public async Task<ServiceResponse<List<NewsFeedDTO>>> GetAll()
        {
            ServiceResponse<List<NewsFeedDTO>> response = new();

            IEnumerable<NewsFeedModel> feeds = await newsFeedRepository.GetAll();
            var newsFeeds = mapper.Map<List<NewsFeedDTO>>(feeds);

            response.Data = newsFeeds;
            response.Success = true;
            return response;
        }

        // Get specific news feeds based on their id
        public async Task<ServiceResponse<List<NewsFeedDTO>>> GetMany(IEnumerable<int> ids)
        {
            ServiceResponse<List<NewsFeedDTO>> response = new();

            foreach (int id in ids)
            {
                if (!NewsFeedExists(id))
                {
                    response.Success = false;
                    response.Message = string.Format("No news feed with id: {0}", id.ToString());
                    return response;
                }
            }

            IEnumerable<NewsFeedModel> feeds = await newsFeedRepository.GetMany(ids);
            var newsFeeds = mapper.Map<List<NewsFeedDTO>>(feeds);

            response.Data = newsFeeds;
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<NewsFeedDTO>> GetSingle(int id)
        {
            ServiceResponse<NewsFeedDTO> response = new();

            if (!NewsFeedExists(id))
            {
                response.Success = false;
                response.Message = string.Format("No news feed with id: {0}", id.ToString());
                return response;
            }

            NewsFeedModel feed = await newsFeedRepository.GetSingle(id);
            NewsFeedDTO newsFeed = mapper.Map<NewsFeedDTO>(feed);

            response.Data = newsFeed;
            response.Success = true;

            return response;
        }

        // Adds new news feed to database.
        // News feeds added this way will always
        // use the default news item builder.
        public async Task<ServiceResponse<NewsFeedDTO>> AddFeed(int userId, string feedName, string feedUrl, string imageDefault)
        {
            ServiceResponse<NewsFeedDTO> response = new();

            // Check if user has credentials for this action
            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }*/

            // Check if feed url already in use
            if (NewsFeedExists(feedUrl))
            {
                response.Success = false;
                response.Message = string.Format("News feed with url: {0} already exists", feedUrl);
                return response;
            }

            // Check if feed url leads to a valid address
            if (ReadFeed(feedUrl) == null)
            {
                response.Success = false;
                response.Message = string.Format("News feed url not valid: {0}", feedUrl);
                return response;
            }

            // Check if image url leads to a valid address
            if (!ImageCheck(imageDefault))
            {
                response.Success = false;
                response.Message = string.Format("Image url not valid: {0}", imageDefault);
                return response;
            }

            NewsFeedModel feed = new()
            {
                FeedName = feedName,
                FeedUrl = feedUrl,
                ImageDefault = imageDefault
            };

            newsFeedRepository.Add(feed);
            newsFeedRepository.Commit();

            response.Success = true;
            response.Data = mapper.Map<NewsFeedDTO>(feed);

            return response;
        }

        public async Task<ServiceResponse<NewsFeedDTO>> DeleteFeed(int feedId, int userId)
        {
            ServiceResponse<NewsFeedDTO> response = new();

            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            */
            var feed = await newsFeedRepository.GetSingle(feedId);
            if (feed == null)
            {
                response.Success = false;
                response.Message = "News feed not found";
                return response;
            }
            var newsFeed = mapper.Map<NewsFeedDTO>(feed);

            newsFeedRepository.Delete(feed);
            newsFeedRepository.Commit();

            response.Success = true;
            response.Data = newsFeed;

            return response;
        }

        // Checks if image url lead to valid address
        // Returns true if status code returned is 200
        // Note: status code 200 only means url leads
        // to a valid adress, not that is leads to an image
        public bool ImageCheck(string imageUrl)
        {
            using HttpClient client = new();
            HttpResponseMessage response = client.GetAsync(imageUrl).Result;
            if (response.IsSuccessStatusCode) return true;
            else return false;
        }

        // Tries to read feed url and return result.
        // Returns null if cannot be read
        public SyndicationFeed? ReadFeed(string feedUrl)
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
                return null;
            }
            return feed;
        }

        public List<NewsItem> GetNewsItems(NewsFeedModel newsFeed)
        {
            List<NewsItem> feedItemList = new();

            var feed = ReadFeed(newsFeed.FeedUrl);
            // If feed cannot be read return empty news item list
            if (feed == null) return feedItemList;

            NewsItemBuilder ItemBuilder = GetNewsItemBuilder(newsFeed);
            
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
            return feedItemList;
        }

        // Returns the relevant news item builder for the feed
        public NewsItemBuilder GetNewsItemBuilder(NewsFeedModel newsFeed)
        {
            // Non-concrete feeds always use the general news item builder
            if(!newsFeed.IsConcrete) return new NewsItemBuilder(newsFeed.ImageDefault);

            // Fetch dictionary object that ties concrete feeds
            // to their specific concrete news item builders
            var feeds = NewsFeedConcrete.GetFeeds();

            // If dedicated news item builder for the concrete feed
            // is not found, returns the general builder
            if(!feeds.ContainsKey(newsFeed.FeedName)) return new NewsItemBuilder(newsFeed.ImageDefault);
            else return feeds[newsFeed.FeedName];
        }
    }
}
