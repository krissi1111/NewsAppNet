using NewsAppNet.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace NewsAppNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        ICommentReplyService commentReplyService;

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

        [Authorize]
        [HttpPost("addComment")]
        public void AddComment([FromForm] int newsId, [FromForm] string commentText)
        {
            int userId = GetUserId();
            commentReplyService.AddComment(newsId, userId, commentText);
        }

        [Authorize]
        [HttpPost("addReply")]
        public void AddReply([FromForm] int newsId, [FromForm] int commentId, [FromForm] string replyText)
        {
            int userId = GetUserId();
            commentReplyService.AddReply(newsId, userId, commentId, replyText);
        }

        [HttpPost("commentList")]
        public ActionResult GetNewsCommentList([FromForm] int newsId)
        {
            int userId = GetUserId();
            var commentList = commentReplyService.GetCommentList(newsId, userId);

            return new JsonResult(commentList);
        }
    }
}
