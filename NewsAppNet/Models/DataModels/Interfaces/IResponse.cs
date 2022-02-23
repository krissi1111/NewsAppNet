namespace NewsAppNet.Models.DataModels.Interfaces
{
    public interface IResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
    }
}
