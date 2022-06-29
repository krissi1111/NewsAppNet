namespace NewsAppNet.Models.DTOs
{
    public class Search
    {
        public string? Title { get; set; } = string.Empty;
        public string? Summary { get; set; } = string.Empty;
        public string DateStart { get; set; } = new DateTime().ToString();
        public string DateEnd { get; set; } = DateTime.Now.ToString();
        public List<int>? NewsFeedIds { get; set; }
    }
}
