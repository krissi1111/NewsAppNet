
using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.DataModels
{
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public IList<Comment>? Comments { get; set; }
        public IList<Reply>? Replies { get; set; }
        public IList<Favorite>? Favorites { get; set; }
    }
}
