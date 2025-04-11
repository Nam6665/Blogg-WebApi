using Blogg.Interface;
using Blogg.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogg.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public CommentController(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (comment == null || string.IsNullOrEmpty(comment.Content) || comment.PostId <= 0)
            {
                return BadRequest("Comment content and PostId are required.");
            }

            var blogPost = await _blogPostRepository.GetPostById(comment.PostId);
            if (blogPost == null)
            {
                return NotFound("Blog post not found.");
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("User  is not authenticated.");
            }

            if (blogPost.UserId == userId.Value)
            {
                return Forbid("You cannot comment on your own post.");
            }

            comment.UserId = userId.Value;
            comment.CreatedAt = DateTime.UtcNow;

            var commentId = await _commentRepository.CreateComment(comment);
            return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = comment.PostId }, commentId);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostId(postId);
            return Ok(comments);
        }

        private int? GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return null;
            }

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}