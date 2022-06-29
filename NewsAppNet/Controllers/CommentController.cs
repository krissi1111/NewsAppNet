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
        readonly ICommentReplyService commentReplyService;
        readonly UserManager<ApplicationUser> userManager;

        public CommentController(
            ICommentReplyService commentReplyService,
            UserManager<ApplicationUser> userManager
            )
        {
            this.commentReplyService = commentReplyService;
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

        // Adds a new comment
        [Authorize]
        [HttpPost("addComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentAddDTO comment)
        {
            int userId = GetUserId();
            comment.UserId = comment.UserId | userId;

            var response = await commentReplyService.AddComment(comment.NewsId, userId, comment.Text);
            
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
            reply.UserId = reply.UserId | userId;

            var response = await commentReplyService.AddReply(reply.NewsId, userId, reply.ParentId, reply.Text);

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
        public async Task<IActionResult> EditComment([FromForm] int commentId, [FromForm] string commentText)
        {
            int userId = GetUserId();
            
            var response = await commentReplyService.EditComment(commentId, userId, commentText);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        /*
        // Changes text of existing reply.
        // Only original reply author or admin allowed.
        [Authorize]
        [HttpPatch("editReply")]
        public async Task<IActionResult> EditReply([FromForm] int replyId, [FromForm] string replyText)
        {
            int userId = GetUserId();

            var response = await commentReplyService.EditReply(replyId, userId, replyText);
            
            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        */
        // Deletes existing comment.
        // Only original comment author or admin allowed.
        [Authorize]
        [HttpPost("deleteComment")]
        public async Task<IActionResult> DeleteComment([FromForm] int commentId)
        {
            int userId = GetUserId();
            
            var response = await commentReplyService.DeleteComment(commentId, userId);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        /*
        // Deletes existing reply.
        // Only original reply author or admin allowed.
        [Authorize]
        [HttpPost("deleteReply")]
        public async Task<IActionResult> DeleteReply([FromForm] int replyId)
        {
            int userId = GetUserId();
            
            var response = await commentReplyService.DeleteReply(replyId, userId);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        */
        [Authorize]
        [HttpPost("restoreComment")]
        public async Task<IActionResult> RestoreComment([FromForm] int commentId)
        {
            int userId = GetUserId();
            
            var response = await commentReplyService.RestoreComment(commentId, userId);
            
            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        /*
        [Authorize]
        [HttpPost("restoreReply")]
        public async Task<IActionResult> RestoreReply([FromForm] int replyId)
        {
            int userId = GetUserId();
            
            var response = await commentReplyService.RestoreReply(replyId, userId);

            if (!response.Success)
            {
                return new BadRequestObjectResult(response.Message);
            }

            return Ok(response.Data);
        }
        */
    }
}
