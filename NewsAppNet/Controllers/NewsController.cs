using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Data.NewsFeeds.Feeds;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        INewsService newsService;
        ICommentReplyService commentReplyService;
        IFavoriteService favoriteService;

        public NewsController(
            INewsService newsService,
            ICommentReplyService commentReplyService,
            IFavoriteService favoriteService
            ) 
        { 
            this.newsService = newsService;
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

        [HttpGet]
        public ActionResult<List<NewsItemView>> GetNews()
        {
            ServiceResponse<List<NewsItemView>> serviceResponse = newsService.GetNewsAll();

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        // Returns news items based on search criteria
        [HttpPost("search")]
        public ActionResult<List<NewsItemView>> GetNewsSearch([FromForm] Search search)
        {
            ServiceResponse<List<NewsItemView>> serviceResponse = newsService.GetNewsSearch(search);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [Authorize]
        [HttpGet("Add")]
        public ActionResult<List<NewsItemView>> AddNews()
        {
            int userId = GetUserId();
            ServiceResponse<List<NewsItemView>> serviceResponse = newsService.AddNews(userId);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [Authorize]
        [HttpPost("delete")]
        public ActionResult<NewsItemView> Delete([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<NewsItemView> serviceResponse = newsService.DeleteNews(newsId, userId);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [HttpPost("commentList")]
        public ActionResult<List<CommentView>> GetNewsCommentList([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<List<CommentView>> serviceResponse = commentReplyService.GetCommentList(newsId, userId);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [Authorize]
        [HttpPost("favAddRemove")]
        public ActionResult<string> FavAddRemove([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<string> serviceResponse = favoriteService.AddRemoveFavorite(newsId, userId);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [Authorize]
        [HttpGet("userFav")]
        public ActionResult<List<FavoriteView>> FavUserFavorites()
        {
            int userId = GetUserId();
            ServiceResponse<List<FavoriteView>> serviceResponse = favoriteService.GetUserFavorites(userId);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [HttpGet("popularComments")]
        public ActionResult<Dictionary<string, List<NewsItemView>>> GetPopularNews()
        {
            ServiceResponse<Dictionary<string, List<NewsItemView>>> serviceResponse = newsService.GetPopularNews();

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [Authorize]
        [HttpPost("restore")]
        public ActionResult<NewsItemView> Restore(int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<NewsItemView> serviceResponse = newsService.RestoreNews(newsId, userId);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }
    }
}
