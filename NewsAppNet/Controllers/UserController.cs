using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        readonly IUserService userService;

        public UserController(
            IUserService userService
            )
        {
            this.userService = userService;
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

        private ActionResult HandleResponse(IResponse serviceResponse)
        {
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            else
            {
                return Ok(serviceResponse);
            }
        }

        // Login using username and password
        [HttpPost("login")]
        public ActionResult<UserAuthData> Login([FromForm] string username, [FromForm] string password)
        {
            ServiceResponse<UserAuthData> serviceResponse = userService.Login(username, password);

            return HandleResponse(serviceResponse);
        }

        [HttpPost("register")]
        public ActionResult<UserAuthData> Register([FromForm] User user)
        {
            ServiceResponse<UserAuthData> serviceResponse = userService.Register(user);

            return HandleResponse(serviceResponse);
        }

        // Checks if login token is still valid.
        // If so, returns new token with extended lifespan.
        // Authorize attribute ensures that only valid tokens are allowed through.
        [Authorize]
        [HttpPost("status")]
        public ActionResult<UserAuthData> LoginCheck()
        {
            var userId = GetUserId();
            ServiceResponse<UserAuthData> serviceResponse = userService.LoginToken(userId);

            return HandleResponse(serviceResponse);
        }
    }
}
