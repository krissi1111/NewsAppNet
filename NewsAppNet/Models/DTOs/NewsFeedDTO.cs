namespace NewsAppNet.Models.DTOs
{
    public class NewsFeedDTO
    {
        public int Id { get; set; }
        public string FeedName { get; set; }
        public string FeedUrl { get; set; }
        public string ImageDefault { get; set; }
        public bool IsConcrete { get; set; }
    }
}
