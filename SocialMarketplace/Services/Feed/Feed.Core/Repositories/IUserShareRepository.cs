using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface IUserShareRepository
    {
        Task<long> CountTotalSharesByUserIdAsync(string userId);
        Task<long> CountTotalSharesByPostIdAsync(string postId);
        Task<UserShare> CreateUserShareAsync(UserShare userShare);
        Task<IEnumerable<GroupedUserShare>> GroupUserShareByUserIdAsync(string userId);
    }
}
