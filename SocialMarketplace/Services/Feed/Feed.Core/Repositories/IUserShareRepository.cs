using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface IUserShareRepository
    {
        Task<long> CountTotalSharesByUserIdAsync(string userId);
        Task<long> CountTotalSharesByPostIdAsync(string postId);
        Task<UserShare> CreateUserShareAsync(UserShare userShare);
    }
}
