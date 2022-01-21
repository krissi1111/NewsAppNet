namespace NewsAppNet.Data.NewsFeeds.ItemBuilder
{
    public interface INewsItemBuilder
    {
        string GetTitle();
        string GetSummary();
        string GetLink();
        string GetImage();
        DateTime GetDate();
    }
}
