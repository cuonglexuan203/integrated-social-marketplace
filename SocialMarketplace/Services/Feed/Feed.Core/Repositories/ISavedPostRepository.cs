
using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface ISavedPostRepository
    {
        Task<SavedPost> SavePostAsync(string userId, string postId, CancellationToken token = default);
        Task<bool> RemoveSavedPostAsync(string userId, string postId, CancellationToken token = default);
        Task<IEnumerable<SavedPost>> GetSavedPostsAsync(string userId, CancellationToken token = default);
    }
}
