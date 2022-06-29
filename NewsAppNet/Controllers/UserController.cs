using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly IUserService userService;

        public UserController(
            IUserService userService
            )
        {
            this.userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid || userLogin == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var response = await userService.Login(userLogin);

            if (!response.Success)
            {
                return new BadRequestObjectResult(new { response.Message });
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost("LoginToken")]
        public async Task<IActionResult> LoginToken()
        {
            string username = "";
            if (HttpContext.User.Identity.Name != null)
            {
                username = HttpContext.User.Identity.Name;
            }
            var response = await userService.LoginToken(username);

            if (!response.Success)
            {
                return new BadRequestObjectResult(new { response.Message });
            }

            return Ok(response.Data);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            if (!ModelState.IsValid || userRegister == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var response = await userService.Register(userRegister);

            if (!response.Success)
            {
                return new BadRequestObjectResult(new { response.Message });
            }

            return Ok(response.Data);
        }
    }
}
