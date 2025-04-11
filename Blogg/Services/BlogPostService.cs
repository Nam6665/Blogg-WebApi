using Blogg.Data;
using Blogg.Interface;
using Dapper;

namespace Blogg.Services
{
    public class BlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly DapperContext _context;

        public BlogPostService(IBlogPostRepository postRepository, DapperContext context) 
        {
            _blogPostRepository = postRepository;
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetAllPosts()
        {
            return await _blogPostRepository.GetAllPosts();
        }

        public async Task<BlogPost> GetPostById(int postId)
        {
            return await _blogPostRepository.GetPostById(postId);
        }

        public async Task<int> CreatePost(BlogPost post)
        {
            return await _blogPostRepository.CreatePost(post);
        }

        public async Task<bool> UpdatePost(BlogPost post)
        {
            return await _blogPostRepository.UpdatePost(post);
        }

        public async Task<bool> DeletePost(int postId)
        {
            return await _blogPostRepository.DeletePost(postId);
        }

        public async Task<IEnumerable<BlogPost>> SearchPosts(string title, int? categoryId)
        {
            return await _blogPostRepository.SearchPosts(title, categoryId);
        }
    }
}