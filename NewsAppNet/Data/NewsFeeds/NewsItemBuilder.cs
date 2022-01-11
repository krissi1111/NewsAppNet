using System;
using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public class NewsItemBuilder : INewsItemBuilder
    {
        public SyndicationItem Item;

        public NewsItemBuilder(SyndicationItem item)
        {
            Item = item;
        }

        public virtual string GetTitle()
        {
            return Item.Title.Text;
        }

        public virtual string GetSummary()
        {
            return Item.Summary.Text;
        }

        public virtual string GetLink()
        {
            return Item.Links[0].GetAbsoluteUri().ToString();
        }

        public virtual string GetImage()
        {
            return "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/480px-No_image_available.svg.png";
        }

        public virtual DateTime GetDate()
        {
            return Item.PublishDate.DateTime;
        }
    }
}
