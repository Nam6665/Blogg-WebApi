using Blogg.Models;

namespace Blogg.Interface
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentById(int commentId);
        Task<int> CreateComment(Comment comment);
        Task<bool> DeleteComment(int commentId);
        Task<bool> UpdateComment(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);

    }
}