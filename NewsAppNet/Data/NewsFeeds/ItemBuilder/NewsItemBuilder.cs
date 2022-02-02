using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public class NewsItemBuilder : INewsItemBuilder
    {
        public string NoImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/480px-No_image_available.svg.png";

        public virtual string GetTitle(SyndicationItem Item)
        {
            return Item.Title.Text;
        }

        public virtual string GetSummary(SyndicationItem Item)
        {
            return Item.Summary.Text;
        }

        public virtual string GetLink(SyndicationItem Item)
        {
            return Item.Links[0].GetAbsoluteUri().ToString();
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
