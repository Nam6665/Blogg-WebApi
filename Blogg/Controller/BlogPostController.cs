using Blogg.Interface;
using Blogg.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogg.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly BlogPostService _blogPostService;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostController(BlogPostService blogPostService, ICategoryRepository categoryRepository)
        {
            _blogPostService = blogPostService;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _blogPostService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _blogPostService.GetPostById(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] BlogPost post)
        {
            if (post == null || string.IsNullOrEmpty(post.Title) || string.IsNullOrEmpty(post.Content))
            {
                return BadRequest("Title and content are required.");
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            post.UserId = userId.Value;
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            try
            {
                var postId = await _blogPostService.CreatePost(post);
                if (postId <= 0)
                {
                    return StatusCode(500, "An error occurred while creating the post.");
                }

                post.PostId = postId;

                return CreatedAtAction(nameof(GetPostById), new { id = postId }, post);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating post: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the post.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] BlogPost post)
        {
            if (post == null || post.PostId != id)
            {
                return BadRequest("Post ID mismatch.");
            }

            if (string.IsNullOrEmpty(post.Title) || string.IsNullOrEmpty(post.Content))
            {
                return BadRequest("Title and content are required.");
            }

            var existingPost = await _blogPostService.GetPostById(id);
            if (existingPost == null)
            {
                return NotFound("Blog post not found.");
            }

            var currentUserId = GetCurrentUserId();
            if (existingPost.UserId != currentUserId)
    {
                return Forbid("You do not have permission to edit this post.");
            }
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.CategoryId = post.CategoryId;
            existingPost.UpdatedAt = DateTime.UtcNow;

            var result = await _blogPostService.UpdatePost(existingPost);
            if (!result)
            {
                return StatusCode(500, "An error occurred while updating the post.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _blogPostService.GetPostById(id);
            if (post == null)
            {
                return NotFound("Blog post not found.");
            }

            var currentUserId = GetCurrentUserId(); 

            if (post.UserId != currentUserId)
    {
                return Forbid("You do not have permission to delete this post.");
            }

            var result = await _blogPostService.DeletePost(id);
            if (!result)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }

            return NoContent();
        }
        private int? GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                Console.WriteLine("User ID not found in claims.");
                return null;
            }

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            Console.WriteLine($"Failed to convert User ID '{userIdString}' to int.");
            return null;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string title = null, [FromQuery] int? categoryId = null)
        {
            Console.WriteLine($"Searching for posts with Title: '{title}' and CategoryId: '{categoryId}'");

            var posts = await _blogPostService.SearchPosts(title, categoryId);

            return Ok(posts);
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return Ok(categories);
        }
    }
}
