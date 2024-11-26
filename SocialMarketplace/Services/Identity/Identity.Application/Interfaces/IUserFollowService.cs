using Identity.Application.DTOs;

namespace Identity.Application.Interfaces
{
    public interface IUserFollowService
    {
        Task<bool> FollowUserAsync(string followerId, string followedId, CancellationToken token = default);
        Task<bool> UnfollowUserAsync(string followerId, string followedId, CancellationToken token = default);
        Task<List<UserDetailsResponseDTO>> GetFollowersAsync(string userId, CancellationToken token = default);
        Task<List<UserDetailsResponseDTO>> GetFollowingAsync(string userId, CancellationToken token = default);
        Task<bool> IsFollowingAsync(string followerId, string followedId, CancellationToken token = default);
    }
}
