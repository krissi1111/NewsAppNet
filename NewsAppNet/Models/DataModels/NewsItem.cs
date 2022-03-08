
using NewsAppNet.Models.DataModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NewsAppNet.Models.DataModels
{
    public class NewsItem : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public string Origin { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Summary { get; set; } = string.Empty;

        [Required]
        [Url]
        public string Link { get; set; } = string.Empty;

        [Required]
        [Url]
        public string Image { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }
        public IList<Comment>? Comments { get; set; }
        public IList<Reply>? Replies { get; set; }
        public IList<Favorite>? Favorites { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
