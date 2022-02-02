using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public class DvItemBuilder : NewsItemBuilder
    {
        public override string GetSummary(SyndicationItem Item)
        {
            string summary = Item.Summary.Text;
            int start = summary.IndexOf("<p>");
            start = start + 3;
            int stop = summary.Substring(start).IndexOf("<a class");
            if (stop < 0)
            {
                stop = summary.Length - start;
            }
            return summary.Substring(start, stop);
        }

        public override string GetImage(SyndicationItem Item)
        {
            string summary = Item.Summary.Text;
            int start = summary.IndexOf("src=");
            if (start == -1)
            {
                return NoImage;
            }
            start += 5;
            int stop = summary.Substring(start).IndexOf("class");
            stop -= 2;
            return summary.Substring(start, stop);
        }
    }
}
