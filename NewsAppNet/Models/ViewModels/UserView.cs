using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Models.ViewModels
{
    public class UserView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; }

        public UserView(User user)
        {
            Id = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            FullName = string.Format("{0} {1}", user.FirstName, user.LastName);
            UserType = user.UserType;
        }
    }
}
