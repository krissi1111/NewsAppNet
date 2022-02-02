using System.ServiceModel.Syndication;

namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public interface INewsItemBuilder
    {
        string GetTitle(SyndicationItem Item);
        string GetSummary(SyndicationItem Item);
        string GetLink(SyndicationItem Item);
        string GetImage(SyndicationItem Item);
        DateTime GetDate(SyndicationItem Item);
    }
}
