using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;
using NewsAppNet.Models.DTOs;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        readonly INewsService newsService;
        readonly INewsFeedService newsFeedService;
        readonly ICommentService commentReplyService;
        readonly IFavoriteService favoriteService;

        public NewsController(
            INewsService newsService,
            INewsFeedService newsFeedService,
            ICommentService commentReplyService,
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
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var response = await newsService.GetNewsAll();
            
            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }

        // Returns news items based on search criteria
        [HttpPost("search")]
        public async Task<IActionResult> GetNewsSearch([FromBody] Search search)
        {
            var response = await newsService.GetNewsSearch(search);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost("feeds")]
        public async Task<IActionResult> GetNewsFeeds([FromForm] int[]? newsFeedIds)
        {
            /*ServiceResponse<List<NewsFeedView>> serviceResponse = await newsFeedService.GetFeeds(newsFeedIds);

            return HandleResponse(serviceResponse);*/

            var response = await newsFeedService.GetFeeds(newsFeedIds);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpGet("Add")]
        public async Task<IActionResult> AddNews()
        {
            int userId = GetUserId();
            ServiceResponse<List<NewsItemDTO>> serviceResponse = await newsService.AddNews(userId);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<NewsItemDTO> serviceResponse = await newsService.DeleteNews(newsId, userId);

            return HandleResponse(serviceResponse);
        }

        [HttpPost("commentList")]
        public async Task<IActionResult> GetNewsCommentList([FromForm] int newsId)
        {
            var response = await commentReplyService.GetComments(newsId);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost("favAddRemove")]
        public async Task<IActionResult> FavAddRemove([FromForm] int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<string> serviceResponse = await favoriteService.AddRemoveFavorite(newsId, userId);

            return HandleResponse(serviceResponse);
        }

        [HttpGet("popularComments")]
        public async Task<IActionResult> GetPopularNews()
        {
            var response = await newsService.GetPopularNews();

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost("restore")]
        public async Task<IActionResult> Restore(int newsId)
        {
            int userId = GetUserId();
            ServiceResponse<NewsItemDTO> serviceResponse = await newsService.RestoreNews(newsId, userId);

            return HandleResponse(serviceResponse);
        }
    }
}
