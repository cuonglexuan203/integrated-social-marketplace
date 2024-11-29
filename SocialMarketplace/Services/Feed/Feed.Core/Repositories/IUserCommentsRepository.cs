using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface IUserCommentsRepository
    {
        Task<long> CountTotalCommentsByUserIdAsync(string userId);
        Task<long> CountTotalCommentsByPostIdAsync(string postId);
        Task<IEnumerable<UserComment>> GetUserCommentsByPostId(string postId);
    }
}
