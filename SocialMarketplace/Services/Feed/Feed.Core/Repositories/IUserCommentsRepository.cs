using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface IUserCommentsRepository
    {
        Task<long> CountTotalCommentsByUserIdAsync(string userId);
        Task<long> CountTotalCommentsByPostIdAsync(string postId);
        Task<IEnumerable<UserComment>> GetUserCommentsByPostId(string postId);
        Task<UserComment> CreateUserCommentAsync(UserComment userComment);
        Task<IEnumerable<GroupedUserComment>> GroupUserCommentByUserIdAsync(string userId);

    }
}
