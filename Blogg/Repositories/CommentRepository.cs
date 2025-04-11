using Blogg.Data;
using Blogg.Interface;
using Blogg.Models;
using Dapper;
using System.Data;

namespace Blogg.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DapperContext _context;

        public CommentRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateComment(Comment comment)
        {
            comment.CreatedAt = DateTime.UtcNow;

            var query = "CreateComment";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, new
                {
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt
                }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            var query = "GetCommentsByPostId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Comment>(query, new { PostId = postId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Comment> GetCommentById(int commentId)
        {
            var query = "GetCommentById";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Comment>(query, new { CommentId = commentId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var query = "DeleteComment";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new { CommentId = commentId }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            var query = "UpdateComment";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content
                }, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }
    }
}