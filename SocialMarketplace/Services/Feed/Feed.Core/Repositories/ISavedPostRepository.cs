using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface ISavedPostRepository
    {
        Task<IEnumerable<SavedPost>> GetSavedPosts();
        Task<IEnumerable<SavedPost>> GetSavedPostByUserId(string userId);
        Task<IEnumerable<SavedPost>> GetSavedPostByPostId(string postId);
        Task<bool> CreateSavedPost(SavedPost savedPost);
        //Task<bool> UpdateSavedPost(SavedPost savedPost);
        Task<bool> DeleteSavedPost(string id);
    }
}
