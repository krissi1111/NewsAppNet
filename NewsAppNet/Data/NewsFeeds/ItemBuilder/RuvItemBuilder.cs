using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public class RuvItemBuilder : NewsItemBuilder
    {
        public override string GetImage(SyndicationItem Item)
        {
            string link = GetLink(Item);
            using (HttpClient client = new())
            {
                // Check if fetching link is successful
                HttpResponseMessage response = client.GetAsync(link).Result;
                if (!response.IsSuccessStatusCode) return ImageDefault;

                string linkContent = client.GetStringAsync(link).Result;
                int start = linkContent.IndexOf("https://www.ruv.is/sites/default/files/styles");
                if (start == -1)
                {
                    return ImageDefault;
                }
                int stop = linkContent[start..].IndexOf(".jpg");
                stop += 4;
                return linkContent.Substring(start, stop);
            }
        }
    }
}
