namespace NewsAppNet.Models.DataModels.Interfaces
{
    public interface ICommentReply : IEntityBase
    {
        int NewsItemId { get; set; }
        NewsItem NewsItem { get; set; }
        int UserId { get; set; }
        User User { get; set; }
        string Text { get; set; }
        DateTime Date { get; set; }
        IList<Reply>? Replies { get; set; }
    }
}
