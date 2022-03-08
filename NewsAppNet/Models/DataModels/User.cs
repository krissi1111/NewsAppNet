
using NewsAppNet.Models.DataModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NewsAppNet.Models.DataModels
{
    public class User : IEntityBase
    {
        public int Id { get; set; }

        // Username must start with a letter
        // Length 3 to 20
        // Only letters from english alphabet or numbers
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{2,19}$")]
        public string Username { get; set; } = string.Empty;

        // First name has length 2 to 20
        // Should allow letters from all western alphabets
        [RegularExpression(@"^[a-zA-Zá-þÁ-Þ]{2,20}$")]
        public string FirstName { get; set; } = string.Empty;

        // Last name has length 2 to 20
        // Should allow letters from all western alphabets
        [RegularExpression(@"^[a-zA-Zá-þÁ-Þ]{2,20}$")]
        public string LastName { get; set; } = string.Empty;

        // Password has length 4+
        // Only letters from english alphabet or numbers
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{4,}$")]
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public IList<Comment>? Comments { get; set; }
        public IList<Reply>? Replies { get; set; }
        public IList<Favorite>? Favorites { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
