using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Identity;
using Identity.Infrastructure.Mappers;
using Identity.Infrastructure.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Services
{
    public class UserFollowService : IUserFollowService
    {
        private readonly IdentityContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserFollowService> _logger;

        public UserFollowService(IdentityContext context, UserManager<ApplicationUser> userManager, ILogger<UserFollowService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<bool> FollowUserAsync(string followerId, string followedId, CancellationToken token = default)
        {
            if (followerId == followedId)
            {
                _logger.LogError("Users cannot follow themselves followerId: {followerId}, followedId {followedId}", followerId, followedId);
                throw new BadRequestException("Users cannot follow themselves");
            }

            var follower = await _userManager.FindByIdAsync(followerId);
            var followed = await _userManager.FindByIdAsync(followedId);

            if (follower == null || followed == null)
            {
                _logger.LogError("One or both users not found: {followerId}, followedId {followedId}", followerId, followedId);
                throw new NotFoundException("One or both users not found");
            }

            var existingFollow = await _context.UserFollows.
                AnyAsync(uf =>
                    uf.FollowerId == followerId &&
                    uf.FollowedId == followedId, token);

            if (existingFollow)
            {
                _logger.LogError("Already following this user: {followerId}, followedId {followedId}", followerId, followedId);
                throw new BadRequestException("Already following this user");
            }

            var userFollow = new UserFollow()
            {
                FollowerId = followerId,
                FollowedId = followedId,
            };

            try
            {
                _context.UserFollows.Add(userFollow);
                await _context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the follow relationship.");
                throw new Exception("An error occurred while processing the follow request.", ex);
            }

            return true;
        }

        public async Task<bool> UnfollowUserAsync(string followerId, string followedId, CancellationToken token = default)
        {
            if (followerId == followedId)
            {
                _logger.LogError("Users cannot unfollow themselves. followerId: {followerId}, followedId: {followedId}", followerId, followedId);
                throw new BadRequestException("Users cannot unfollow themselves");
            }

            var follower = await _userManager.FindByIdAsync(followerId);
            var followed = await _userManager.FindByIdAsync(followedId);

            if (follower == null || followed == null)
            {
                _logger.LogError("One or both users not found. followerId: {followerId}, followedId: {followedId}", followerId, followedId);
                throw new NotFoundException("One or both users not found");
            }

            // Check if the user is currently following the followed user
            var existingFollow = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.FollowerId == followerId && uf.FollowedId == followedId, token);

            if (existingFollow == null)
            {
                _logger.LogError("User is not following. followerId: {followerId}, followedId: {followedId}", followerId, followedId);
                throw new BadRequestException("User is not following this user");
            }

            // Remove the follow relationship
            _context.UserFollows.Remove(existingFollow);

            try
            {
                await _context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing the follow relationship.");
                throw new Exception("An error occurred while processing the unfollow request.", ex);
            }

            return true; // Return true indicating the unfollow action was successful
        }


        public async Task<List<UserDetailsResponseDTO>> GetFollowersAsync(string userId, CancellationToken token = default)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Id == userId, token);

            if (!existingUser)
            {
                throw new NotFoundException($"User not found with ID: {userId}");
            }

            var followers = await _context.UserFollows
                .Where(uf => uf.FollowedId == userId)
                .Select(uf => uf.Follower)
                .ToListAsync();

            return IdentityMapper.Mapper.Map<List<UserDetailsResponseDTO>>(followers);
        }


        public async Task<List<UserDetailsResponseDTO>> GetFollowingAsync(string userId, CancellationToken token = default)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Id == userId, token);

            if (!existingUser)
            {
                throw new NotFoundException($"User not found with ID: {userId}");
            }

            var followeds = await _context.UserFollows
                .Where(uf => uf.FollowerId == userId)
                .Select(uf => uf.Followed)
                .ToListAsync();

            return IdentityMapper.Mapper.Map<List<UserDetailsResponseDTO>>(followeds);
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followedId, CancellationToken token = default)
        {
            if (followerId == followedId)
                return false;
            return await _context.UserFollows.AnyAsync(uf => uf.FollowerId ==  followerId && uf.FollowedId == followedId, token);
        }


    }
}
