namespace NewsAppNet.Data.NewsFeeds
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
