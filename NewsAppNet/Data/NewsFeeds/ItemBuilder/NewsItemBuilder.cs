using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public class NewsItemBuilder : INewsItemBuilder
    {
        public string NoImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/480px-No_image_available.svg.png";

        public virtual string GetTitle(SyndicationItem Item)
        {
            if (Item.Title == null) return "NO TITLE";
            else return Item.Title.Text;
        }

        public virtual string GetSummary(SyndicationItem Item)
        {
            var summary = Item.Summary.Text;
            if (summary == null) return "NO SUMMARY";
            else return summary;
        }

        public virtual string GetLink(SyndicationItem Item)
        {
            var link = Item.Links[0].GetAbsoluteUri().ToString();
            if (link == null) return "NO LINK";
            else return link;
        }

        public virtual string GetImage(SyndicationItem Item)
        {
            return NoImage;
        }

        public virtual DateTime GetDate(SyndicationItem Item)
        {
            return Item.PublishDate.DateTime;
        }
    }
}
