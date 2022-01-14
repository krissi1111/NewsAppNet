using NewsAppNet.Models.ViewModels;

namespace NewsAppNet.Models.DataModels
{
    public class UserAuthData
    {
        public string Token { get; set; }
        public string TokenExpirationTime { get; set; }
        public UserView User { get; set; }
    }
}
