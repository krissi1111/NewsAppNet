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

        public int GetUserId()
        {
            int userId = -1;
            if (HttpContext.User.Identity.Name != null)
            {
                userId = Int32.Parse(HttpContext.User.Identity.Name);
            }
            return userId;
        }

        // Login using username and password
        [HttpPost("login")]
        public ActionResult<UserAuthData> Login([FromForm] string username, [FromForm] string password)
        {
            ServiceResponse<UserAuthData> serviceResponse = userService.Login(username, password);
            
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }

        [HttpPost("register")]
        public ActionResult<UserAuthData> Register([FromForm] User user)
        {
            ServiceResponse<UserAuthData> serviceResponse = userService.Register(user);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
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

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }
            else
            {
                return Ok(serviceResponse.Data);
            }
        }
    }
}
