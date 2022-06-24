using NewsAppNet.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DataModels.Interfaces;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        readonly ICommentReplyService commentReplyService;

        public CommentController(
            ICommentReplyService commentReplyService
            )
        {
            this.commentReplyService = commentReplyService;
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

        // Adds a new comment
        [Authorize]
        [HttpPost("addComment")]
        public async Task<IActionResult> AddComment([FromForm] int newsId, [FromForm] string commentText)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.AddComment(newsId, userId, commentText);

            return HandleResponse(serviceResponse);
        }

        // Adds a new reply
        [Authorize]
        [HttpPost("addReply")]
        public async Task<IActionResult> AddReply([FromForm] int newsId, [FromForm] int commentId, [FromForm] string replyText)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.AddReply(newsId, userId, commentId, replyText);

            return HandleResponse(serviceResponse);
        }

        // Changes text of existing comment.
        // Only original comment author or admin allowed.
        [Authorize]
        [HttpPatch("editComment")]
        public async Task<IActionResult> EditComment([FromForm] int commentId, [FromForm] string commentText)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.EditComment(commentId, userId, commentText);

            return HandleResponse(serviceResponse);
        }

        // Changes text of existing reply.
        // Only original reply author or admin allowed.
        [Authorize]
        [HttpPatch("editReply")]
        public async Task<IActionResult> EditReply([FromForm] int replyId, [FromForm] string replyText)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.EditReply(replyId, userId, replyText);

            return HandleResponse(serviceResponse);
        }

        // Deletes existing comment.
        // Only original comment author or admin allowed.
        [Authorize]
        [HttpPost("deleteComment")]
        public async Task<IActionResult> DeleteComment([FromForm] int commentId)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.DeleteComment(commentId, userId);

            return HandleResponse(serviceResponse);
        }

        // Deletes existing reply.
        // Only original reply author or admin allowed.
        [Authorize]
        [HttpPost("deleteReply")]
        public async Task<IActionResult> DeleteReply([FromForm] int replyId)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.DeleteReply(replyId, userId);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpPost("restoreComment")]
        public async Task<IActionResult> RestoreComment([FromForm] int commentId)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.RestoreComment(commentId, userId);

            return HandleResponse(serviceResponse);
        }

        [Authorize]
        [HttpPost("restoreReply")]
        public async Task<IActionResult> RestoreReply([FromForm] int replyId)
        {
            int userId = GetUserId();
            ServiceResponse<CommentView> serviceResponse = await commentReplyService.RestoreReply(replyId, userId);

            return HandleResponse(serviceResponse);
        }
    }
}
