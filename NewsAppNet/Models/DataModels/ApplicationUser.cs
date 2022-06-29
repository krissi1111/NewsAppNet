using Microsoft.AspNetCore.Identity;
using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Models.DataModels
{
    public class ApplicationUser : IdentityUser<int>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public IEnumerable<Favorite>? Favorites { get; set; }
    }
}
