using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        INewsItemRepository newsItemRepository;

        public NewsController(
            INewsItemRepository newsItemRepository
            ) 
        { 
            this.newsItemRepository = newsItemRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var item = new NewsItem();
            item.Date = DateTime.Now;
            item.Title = "title";
            item.Summary = "summary";

            newsItemRepository.Add(item);
            newsItemRepository.Commit();

            return new JsonResult(newsItemRepository.GetAll());
        }
    }
}
