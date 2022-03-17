using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        readonly INewsService newsService;
        readonly INewsFeedService newsFeedService;
        readonly ICommentReplyService commentReplyService;
        readonly IFavoriteService favoriteService;

        public NewsController(
            INewsService newsService,
            INewsFeedService newsFeedService,
            ICommentReplyService commentReplyService,
            IFavoriteService favoriteService
            ) 
        { 
            this.newsService = newsService;
            this.newsFeedService = newsFeedService;
            this.commentReplyService = commentReplyService;
            this.favoriteService = favoriteService;
        }

        public int GetUserId()
        {
            int userId = -1;
            if (HttpContext.User.Identity.Name != null)
            {
                userId = Int32.Parse(HttpContext.User.Identity.Name);
            }
            return userId;
        }

        private ActionResult HandleResponse(IResponse serviceResponse)
        {
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            else
            {
                return Ok(serviceResponse);
            }
        }

        [HttpGet]
        public ActionResult<List<NewsItemView>> GetNews()
        {
            ServiceResponse<List<NewsItemView>> serviceResponse = newsService.GetNewsAll();

            return HandleResponse(serviceResponse);
        }

        // Returns news items based on search criteria
        [HttpPost("search")]
        public ActionResult<List<NewsItemView>> GetNewsSearch([FromForm] Search search)
        {
            ServiceResponse<List<NewsItemView>> serviceResponse = newsService.GetNewsSearch(search);

            return HandleResponse(serviceResponse);
        }

        [HttpPost("feeds")]
        public ActionResult<List<NewsFeedView>> GetNewsFeeds([FromForm] int[]? newsFeedIds)
        {
            ServiceResponse<List<NewsFeedView>> serviceResponse = newsFeedService.GetFeeds(newsFeedIds);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpGet("Add")]
        public ActionResult<List<NewsItemView>> AddNews()
        {
            int userId = GetUserId();
            ServiceResponse<List<NewsItemView>> serviceResponse = newsService.AddNews(userId);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpPost("delete")]
        public ActionResult<NewsItemView> Delete([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<NewsItemView> serviceResponse = newsService.DeleteNews(newsId, userId);

            return HandleResponse(serviceResponse);
        }

        [HttpPost("commentList")]
        public ActionResult<List<CommentView>> GetNewsCommentList([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<List<CommentView>> serviceResponse = commentReplyService.GetCommentList(newsId, userId);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpPost("favAddRemove")]
        public ActionResult<string> FavAddRemove([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<string> serviceResponse = favoriteService.AddRemoveFavorite(newsId, userId);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpGet("userFav")]
        public ActionResult<List<FavoriteView>> FavUserFavorites()
        {
            int userId = GetUserId();
            ServiceResponse<List<FavoriteView>> serviceResponse = favoriteService.GetUserFavorites(userId);
            
            return HandleResponse(serviceResponse);
        }

        [HttpGet("popularComments")]
        public ActionResult<Dictionary<string, List<NewsItemView>>> GetPopularNews()
        {
            ServiceResponse<Dictionary<string, List<NewsItemView>>> serviceResponse = newsService.GetPopularNews();

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpPost("restore")]
        public ActionResult<NewsItemView> Restore(int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<NewsItemView> serviceResponse = newsService.RestoreNews(newsId, userId);

            return HandleResponse(serviceResponse);
        }
    }
}
