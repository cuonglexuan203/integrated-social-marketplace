using Chat.Core.Entities;

namespace Chat.Application.Interfaces.HttpClients
{
    public interface IIdentityService
    {
        Task<User> GetUserDetailsAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool?> IsUserFollowingAsync(string followerId, string followedId, CancellationToken cancellationToken = default);
    }
}
