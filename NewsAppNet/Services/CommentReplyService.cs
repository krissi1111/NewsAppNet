﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.DTOs;
using NewsAppNet.Services.Interfaces;

namespace NewsAppNet.Services
{
    public class CommentReplyService : ICommentReplyService
    {
        readonly ICommentRepository commentRepository;
        readonly UserManager<ApplicationUser> userManager;
        readonly IMapper mapper;
        public CommentReplyService(
            ICommentRepository commentRepository, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper
            )
        {
            this.commentRepository = commentRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // Returns all comments for given news item.
        public async Task<ServiceResponse<List<CommentDTO>>> GetComments(int newsId)
        {
            ServiceResponse<List<CommentDTO>> response = new();

            var comments = await commentRepository.GetManyInclude(t => t.NewsItemId == newsId, t => t.Replies, t => t.User);
            
            var commentList = mapper.Map<List<CommentDTO>>(comments);

            response.Data = commentList;
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<CommentDTO>> AddComment(int newsId, int userId, string commentText)
        {
            ServiceResponse<CommentDTO> response = new();

            //var user = userService.GetUser(userId);
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            Comment comment = new()
            {
                NewsItemId = newsId,
                UserId = userId,
                Text = commentText
            };

            commentRepository.Add(comment);
            commentRepository.Commit();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            response.Success = true;
            response.Data = commentDTO;

            return response;
        }

        /*public async Task<ServiceResponse<CommentDTO>> AddReply(int newsId, int userId, int commentId, string commentText)
        {
            ServiceResponse<CommentDTO> response = new();

            var user = userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            Reply reply = new()
            {
                NewsItemId = newsId,
                UserId = userId,
                CommentId = commentId,
                Text = commentText
            };

            replyRepository.Add(reply);
            replyRepository.Commit();

            var replyDTO = mapper.Map<CommentDTO>(reply);

            response.Success = true;
            response.Data = replyDTO;

            return response;
        }
        */
        // Used for editing already existing comments.
        // Users can only edit their own comments.
        // Admin can edit any comment.

        public async Task<ServiceResponse<CommentDTO>> AddReply(int newsId, int userId, int parentId, string commentText)
        {
            ServiceResponse<CommentDTO> response = new();

            Comment comment = new()
            {
                NewsItemId = newsId,
                UserId = userId,
                ParentId = parentId,
                Text = commentText,
                TopLevelComment = false
            };

            commentRepository.Add(comment);
            commentRepository.Commit();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            response.Success = true;
            response.Data = commentDTO;

            return response;
        }

        public async Task<ServiceResponse<CommentDTO>> EditComment(int commentId, int userId, string commentText)
        {
            ServiceResponse<CommentDTO> response = new();

            var comment = await commentRepository.GetSingle(commentId);
            if (comment == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", commentId);
                return response;
            }

            //var user = await userService.GetUser(userId);
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only edit their own comments
            // Admin can edit any comment
            else if(!(user.Id == comment.UserId/* || user.UserType == "Admin"*/))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            comment.Text = commentText;
            commentRepository.Update(comment);
            commentRepository.Commit();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            response.Success = true;
            response.Data = commentDTO;

            return response;
        }
        /*
        // Used for editing already existing replies.
        // Users can only edit their own replies.
        // Admin can edit any reply.
        public async Task<ServiceResponse<CommentDTO>> EditReply(int replyId, int userId, string commentText)
        {
            ServiceResponse<CommentDTO> response = new();

            var reply = await replyRepository.GetSingle(replyId);
            if (reply == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", replyId);
                return response;
            }

            var user = await userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only edit their own comments
            // Admin can edit any comment
            else if (!(user.Id == reply.UserId || user.UserType == "Admin"))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            if (string.IsNullOrEmpty(commentText))
            {
                response.Success = false;
                response.Message = "Comment text is empty";
                return response;
            }

            reply.Text = commentText;
            replyRepository.Update(reply);
            replyRepository.Commit();

            var replyDTO = mapper.Map<CommentDTO>(reply);

            response.Success = true;
            response.Data = replyDTO;

            return response;
        }
        */
        // Used for deleting comments.
        // Users can only delete their own comments.
        // Admin can delete any comment.
        public async Task<ServiceResponse<CommentDTO>> DeleteComment(int commentId, int userId)
        {
            ServiceResponse<CommentDTO> response = new();

            var comment = await commentRepository.GetSingle(commentId);
            if (comment == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", commentId);
                return response;
            }

            //var user = await userService.GetUser(userId);
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only delete their own comments
            // Admin can delete any comment
            else if (!(user.Id == comment.UserId /*|| user.UserType == "Admin"*/))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            var commentDTO = mapper.Map<CommentDTO>(comment);

            response.Data = commentDTO;

            commentRepository.Delete(comment);
            commentRepository.Commit();

            response.Success = true;

            return response;
        }
        /*
        // Used for deleting replies.
        // Users can only delete their own replies.
        // Admin can delete any reply.
        public async Task<ServiceResponse<CommentDTO>> DeleteReply(int replyId, int userId)
        {
            ServiceResponse<CommentDTO> response = new();

            var reply = await replyRepository.GetSingle(replyId);
            if (reply == null)
            {
                response.Success = false;
                response.Message = string.Format("Reply {0} not found", replyId);
                return response;
            }

            var user = await userService.GetUser(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Must be logged in to perform this action";
                return response;
            }
            // Users can only delete their own replies
            // Admin can delete any reply
            else if (!(user.Id == reply.UserId || user.UserType == "Admin"))
            {
                response.Success = false;
                response.Message = "User is not authorized to perform this action";
                return response;
            }

            response.Data = mapper.Map<CommentDTO>(reply);

            replyRepository.Delete(reply);
            replyRepository.Commit();

            response.Success = true;

            return response;
        }
        */
        // Used for restoring soft deleted comments
        public async Task<ServiceResponse<CommentDTO>> RestoreComment(int commentId, int userId)
        {
            ServiceResponse<CommentDTO> response = new();

            var comment = await commentRepository.GetSingle(commentId);
            if (comment == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", commentId);
                return response;
            }

            //var currentUser = await userService.GetUser(userId);
            var currentUser = await userManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            /*else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }*/

            commentRepository.Update(comment);
            commentRepository.Commit();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            response.Success = true;
            response.Data = commentDTO;

            return response;
        }
        /*
        // Used for restoring soft deleted replies
        public async Task<ServiceResponse<CommentDTO>> RestoreReply(int replyId, int userId)
        {
            ServiceResponse<CommentDTO> response = new();

            var reply = await replyRepository.GetSingle(replyId);
            if (reply == null)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} not found", replyId);
                return response;
            }
            else if (!reply.IsDeleted)
            {
                response.Success = false;
                response.Message = string.Format("Comment {0} is not soft deleted", replyId);
                return response;
            }

            var currentUser = await userService.GetUser(userId);
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }
            else if (currentUser.UserType != "Admin")
            {
                response.Success = false;
                response.Message = "This action is only for admins";
                return response;
            }

            reply.IsDeleted = false;
            replyRepository.Update(reply);
            replyRepository.Commit();

            var replyView = mapper.Map<CommentDTO>(reply);

            response.Success = true;
            response.Data = replyView;

            return response;
        }
        */
    }
}
