using NewsAppNet.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;
using NewsAppNet.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        readonly ICommentService commentService;
        readonly UserManager<ApplicationUser> userManager;

        public CommentController(
            ICommentService commentService,
            UserManager<ApplicationUser> userManager
            )
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        public int GetUserId()
        {
            int userId = -1;
            if (HttpContext.User.Identity.Name != null)
            {
                var userName = HttpContext.User.Identity.Name;
                var user = userManager.FindByNameAsync(userName).Result;
                userId = user.Id;
            }
            return userId;
        }
        /*
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
        */
        // Adds a new comment
        [Authorize]
        [HttpPost("addComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentAddDTO comment)
        {
            int userId = GetUserId();

            var response = await commentService.AddComment(comment.NewsId, userId, comment.Text);
            
            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        /*
        // Adds a new reply
        [Authorize]
        [HttpPost("addReply")]
        public async Task<IActionResult> AddReply([FromForm] int newsId, [FromForm] int commentId, [FromForm] string replyText)
        {
            int userId = GetUserId();

            var response = await commentReplyService.AddReply(newsId, userId, commentId, replyText);
            
            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        */
        [Authorize]
        [HttpPost("addReply")]
        public async Task<IActionResult> AddReply([FromBody] ReplyAddDTO reply)
        {
            int userId = GetUserId();

            var response = await commentService.AddReply(reply.NewsId, userId, reply.ParentId, reply.Text);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }


        // Changes text of existing comment.
        // Only original comment author or admin allowed.
        [Authorize]
        [HttpPatch("editComment")]
        public async Task<IActionResult> EditComment([FromBody] CommentEditDTO comment)
        {
            int userId = GetUserId();
            
            var response = await commentService.EditComment(comment.Id, userId, comment.Text);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }

        // Deletes existing comment.
        // Only original comment author or admin allowed.
        [Authorize]
        [HttpPost("deleteComment")]
        public async Task<IActionResult> DeleteComment([FromBody] CommentDeleteDTO comment)
        {
            int userId = GetUserId();
            
            var response = await commentService.DeleteComment(comment.Id, userId);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
    }
}
