using Blogg.Interface;
using Blogg.Models;

namespace Blogg.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<int> CreateComment(Comment comment)
        {
            return await _commentRepository.CreateComment(comment);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBlogPostId(int blogPostId)
        {
            return await _commentRepository.GetCommentsByPostId(blogPostId);
        }
    }
}