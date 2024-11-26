
using Feed.Core.Entities;

namespace Feed.Application.Interfaces.HttpClients
{
    public interface IIdentityService
    {
        Task<User> GetUserDetailsAsync(string userId);
        Task<bool?> IsUserFollowingAsync(string followerId, string followedId, CancellationToken cancellationToken = default);
    }
}
