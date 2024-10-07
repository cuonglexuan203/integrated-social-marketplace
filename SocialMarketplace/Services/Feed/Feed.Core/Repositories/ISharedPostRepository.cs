
using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface ISharedPostRepository
    {
        Task<IEnumerable<SharedPost>> GetSharedPosts();
        Task<SharedPost> GetSharedPost(string id);
        Task<IEnumerable<SharedPost>> GetSharedPostByUserId(string userId);
        Task<IEnumerable<SharedPost>> GetSharedPostByTitle(string title);
        Task<IEnumerable<SharedPost>> GetSharedPostBySharedPostId(string sharedPostId);
        Task<bool> CreateSharedPost(SharedPost post);
        Task<bool> UpdateSharedPost(SharedPost post);
        Task<bool> DeleteSharedPost(string id);
    }
}
