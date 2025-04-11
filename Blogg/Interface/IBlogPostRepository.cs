namespace Blogg.Interface
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllPosts();
        Task<BlogPost> GetPostById(int postId);
        Task<int> CreatePost(BlogPost post);
        Task<bool> UpdatePost(BlogPost post);
        Task<bool> DeletePost(int postId);
        Task<IEnumerable<BlogPost>> SearchPosts(string title, int? categoryId);
    }
}