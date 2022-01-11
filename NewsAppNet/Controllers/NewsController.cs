using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
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
            var item = new NewsItem();
            item.Date = DateTime.Now;
            item.Title = "title";
            item.Summary = "summary";

            newsService.AddNews(item);

            return new JsonResult(newsService.GetNews());
        }
    }
}
