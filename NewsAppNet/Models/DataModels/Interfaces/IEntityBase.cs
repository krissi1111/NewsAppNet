namespace NewsAppNet.Models.DataModels.Interfaces
{
    public interface IEntityBase
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
