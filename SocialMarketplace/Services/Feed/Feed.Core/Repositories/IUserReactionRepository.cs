using Feed.Core.;
using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface IUserReactionRepository
    {
        Task<long> CountTotalReactionsByUserIdAsync(string userId);
        Task<long> CountTotalReactionsByPostIdAsync(string postId);
        Task<UserReaction> CreateUserReactionAsync(UserReaction userReaction);
        Task<IEnumerable<GroupedUserReaction>> GroupUserReactionByUserIdAsync(string userId);
    }
}
