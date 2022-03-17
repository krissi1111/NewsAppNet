using NewsAppNet.Models.DataModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NewsAppNet.Models.DataModels
{
    public class NewsFeedModel : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public string FeedName { get; set; } = string.Empty;

        [Required]
        [Url]
        public string FeedUrl { get; set; } = string.Empty;

        [Required]
        [Url]
        public string ImageDefault { get; set; } = string.Empty;

        // Concrete news feeds use their own concrete implementation 
        // of NewsItemBuilder instead of the general implementation
        public bool IsConcrete { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public IList<NewsItem>? NewsItems { get; set; }
    }
}
