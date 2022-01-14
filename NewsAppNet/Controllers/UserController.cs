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
        public UserController(
            IUserService userService
            )
        {
            this.userService = userService;
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
            var id = HttpContext.User.Identity.Name;
            var user = userService.GetUser(Int32.Parse(id));

            return user;
            //return new JsonResult("User is logged in");
        }
    }
}
