using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public class MblItemBuilder : NewsItemBuilder
    {
        public override string GetSummary(SyndicationItem Item)
        {
            string summary = Item.Summary.Text;
            int start = summary.IndexOf(">");
            start += 2;
            int end = summary.Length - start - 1;
            return summary.Substring(start, end);
        }

        public override string GetImage(SyndicationItem Item)
        {
            string summary = Item.Summary.Text;
            int start = summary.IndexOf("https://cdn.mbl.is/frimg");
            if (start == -1)
            {
                return NoImage;
            }
            int stop = summary[start..].IndexOf(".jpg");
            stop += 4;
            return summary.Substring(start, stop);
        }
    }
}
