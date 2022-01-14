using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Data.NewsFeeds;
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

        public NewsController(
            INewsService newsService
            ) 
        { 
            this.newsService = newsService;
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
            RuvFeed visirFeed = new();

            List<NewsItemView> items = new List<NewsItemView>();
            List<NewsItem> newsFeedItems = visirFeed.GetNewsItems();

            foreach(NewsItem item in newsFeedItems)
            {
                newsService.AddNews(item);
                items.Add(new NewsItemView(item));
            }

            return new JsonResult(items);
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
    }
}
