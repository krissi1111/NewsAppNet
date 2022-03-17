using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Models.ViewModels
{
    public class NewsItemView
    {
        public int Id { get; set; }
        public int NewsFeedId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }

        public NewsItemView(NewsItem newsItem)
        {
            Id = newsItem.Id;
            NewsFeedId = newsItem.NewsFeedId;
            Title = newsItem.Title;
            Summary = newsItem.Summary;
            Link = newsItem.Link;
            Image = newsItem.Image;
            Date = newsItem.Date;
            IsDeleted = newsItem.IsDeleted;
        }
    }
}
