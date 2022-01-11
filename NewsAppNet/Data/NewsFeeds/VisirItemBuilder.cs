using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds
{
    public class VisirItemBuilder : NewsItemBuilder
    {
        public VisirItemBuilder(SyndicationItem item) : base(item) { }

        public override string GetImage()
        {
            string link = GetLink();
            using (HttpClient client = new())
            {
                string linkContent = client.GetStringAsync(link).Result;
                int start = linkContent.IndexOf("https://www.visir.is/i/");
                if (start == -1)
                {
                    return NoImage;
                }
                int stop = linkContent[start..].IndexOf(".jpg");
                stop += 4;
                return linkContent.Substring(start, stop);
            }
        }
    }
}
