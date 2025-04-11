using Blogg.Data;
using Blogg.Interface;
using Blogg.Models;
using Dapper;
using System.Data;

namespace Blogg.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly DapperContext _context;

        public BlogPostRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePost(BlogPost post)
        {
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            var query = "CreatePost";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, new
                {
                    UserId = post.UserId,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    CategoryId = post.CategoryId
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<BlogPost>> GetAllPosts()
        {
            var query = "GetAllPosts";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<BlogPost>(query, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<BlogPost> GetPostById(int postId)
        {
            var query = "GetPostById";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<BlogPost>(query, new { PostId = postId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> UpdatePost(BlogPost post)
        {
            var query = "UpdatePost";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    CategoryId = post.CategoryId,
                    UpdatedAt = DateTime.UtcNow
                }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeletePost(int postId)
        {
            var query = "DeletePost";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new { PostId = postId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<BlogPost>> SearchPosts(string title, int? categoryId)
        {
            var query = "SearchPosts";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<BlogPost>(query, new
                {
                    Title = title,
                    CategoryId = categoryId
                }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}