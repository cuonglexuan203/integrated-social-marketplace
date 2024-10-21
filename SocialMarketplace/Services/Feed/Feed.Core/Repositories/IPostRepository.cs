
using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPost(string id);
        Task<IEnumerable<Post>> GetPostByUserId(string userId);
        Task<IEnumerable<Post>> GetPostByTitle(string title);
        Task<Post> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(string id);
    }
}
