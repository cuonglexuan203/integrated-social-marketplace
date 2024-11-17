
using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetPost(string id);
        //Task<IEnumerable<Post>> GetPostByUserId(string userId);
        Task<Post> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(string id);
    }
}
