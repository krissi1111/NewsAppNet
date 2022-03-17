using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Models.ViewModels
{
    public class NewsFeedView
    {
        public int Id { get; set; }
        public string FeedName { get; set; }
        public string FeedUrl { get; set; }
        public string ImageDefault { get; set; }
        public bool IsConcrete { get; set; }

        public NewsFeedView(NewsFeedModel newsFeedModel)
        {
            Id = newsFeedModel.Id;
            FeedUrl = newsFeedModel.FeedUrl;
            FeedName = newsFeedModel.FeedName;
            ImageDefault = newsFeedModel.ImageDefault;
            IsConcrete = newsFeedModel.IsConcrete;
        }
    }
}
