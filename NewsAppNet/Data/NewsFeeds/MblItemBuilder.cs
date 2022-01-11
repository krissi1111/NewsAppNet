using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public class MblItemBuilder : NewsItemBuilder
    {
        public MblItemBuilder(SyndicationItem item) : base(item) { }

        public override string GetSummary()
        {
            string summary = Item.Summary.Text;
            int start = summary.IndexOf(">");
            start += 2;
            int end = summary.Length - start - 1;
            return summary.Substring(start, end);
        }

        public override string GetImage()
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
