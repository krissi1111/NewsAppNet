using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserService userService;
        IAuthService authService;

        public UserController(
            IUserService userService,
            IAuthService authService
            )
        {
            this.userService = userService;
            this.authService = authService;
        }

        public int GetUserId()
        {
            int userId = -1;
            if (HttpContext.User.Identity.Name != null)
            {
                userId = Int32.Parse(HttpContext.User.Identity.Name);
            }
            return userId;
        }

        [HttpPost("login")]
        public ActionResult<UserAuthData> Login([FromForm] User user)
        {
            return userService.Login(user);
        }

        [HttpPost("register")]
        public ActionResult<UserAuthData> Register([FromForm] User user)
        {
            return userService.Register(user);
        }

        [Authorize]
        [HttpPost("status")]
        public ActionResult<UserAuthData> LoginCheck()
        {
            var id = GetUserId();
            var user = userService.GetUser(id);
            var userAuth = authService.GetUserAuthData(user);

            return userAuth;
            //return new JsonResult("User is logged in");
        }
    }
}
