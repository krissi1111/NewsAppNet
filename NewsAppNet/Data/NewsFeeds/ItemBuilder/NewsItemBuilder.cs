using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public class NewsItemBuilder : INewsItemBuilder
    {
        public string ImageDefault { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/480px-No_image_available.svg.png";

        public NewsItemBuilder(string image)
        {
            ImageDefault = image;
        }

        public NewsItemBuilder() { }

        public virtual string GetTitle(SyndicationItem item)
        {
            if (item.Title == null) return "NO TITLE";
            else return item.Title.Text;
        }

        public virtual string GetSummary(SyndicationItem item)
        {
            if (item.Summary == null) return "NO SUMMARY";
            else return item.Summary.Text;
        }

        public virtual string GetLink(SyndicationItem item)
        {
            if (item.Links[0] == null) return "NO LINK";
            else return item.Links[0].GetAbsoluteUri().ToString();
        }

        public virtual string GetImage(SyndicationItem item)
        {
            return ImageDefault;
        }

        public virtual DateTime GetDate(SyndicationItem item)
        {
            return item.PublishDate.DateTime;
        }
    }
}
