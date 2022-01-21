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
        public ActionResult Get()
        {
            return new JsonResult(newsService.GetNews());
        }

        [HttpPost("search")]
        public ActionResult GetNewsSearch([FromForm] Search search)
        {
            var viewList = newsService.GetNewsSearch(search);
            return new JsonResult(viewList);
        }

        [HttpGet("Add")]
        public ActionResult AddNews()
        {
            return new JsonResult(newsService.AddNews());
        }

        [HttpGet("delete")]
        public void Delete()
        {
            var news = newsService.GetNews();
            foreach(var item in news)
            {
                newsService.RemoveNews(item.Id);
            }
        }

        [HttpPost("commentList")]
        public ActionResult GetNewsCommentList([FromForm] int newsId)
        {
            int userId = GetUserId();
            var commentList = commentReplyService.GetCommentList(newsId, userId);

            return new JsonResult(commentList);
        }

        [Authorize]
        [HttpPost("favAddRemove")]
        public ActionResult favAddRemove([FromForm] int newsId)
        {
            int userId = GetUserId();
            if (userId == -1)
            {
                return BadRequest(new { Login = "User not logged in" });
            }

            favoriteService.AddRemoveFavorite(newsId, userId);
            return favUserFavorites();
            //return Ok();
        }

        [Authorize]
        [HttpGet("userFav")]
        public ActionResult favUserFavorites()
        {
            int userId = GetUserId();
            if (userId == -1)
            {
                return BadRequest(new { Login = "User not logged in" });
            }

            var favList = favoriteService.GetUserFavorites(userId);

            return new JsonResult(favList);
        }

        [HttpGet("popularComments")]
        public ActionResult GetPopularNews()
        {
            var popularNews = newsService.GetPopularNews();
            return new JsonResult(popularNews);
        }
    }
}
