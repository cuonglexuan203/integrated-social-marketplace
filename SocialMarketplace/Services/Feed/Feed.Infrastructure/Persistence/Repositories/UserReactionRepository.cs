using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Core.ValueObjects;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class UserReactionRepository : IUserReactionRepository
    {
        private readonly ILogger<UserReactionRepository> _logger;
        private readonly IMongoCollection<UserReaction> _userReactions;

        public UserReactionRepository(ILogger<UserReactionRepository> logger, IFeedContext feedContext)
        {
            _logger = logger;
            _userReactions = feedContext.UserReactions;
        }
        public async Task<long> CountTotalReactionsByPostIdAsync(string postId)
        {
            return await _userReactions.CountDocumentsAsync(x => x.PostId == postId);
        }

        public async Task<long> CountTotalReactionsByUserIdAsync(string userId)
        {
            return await _userReactions.CountDocumentsAsync(_ => _.UserId == userId);
        }

        public async Task<UserReaction> CreateUserReactionAsync(UserReaction userReaction)
        {
            await _userReactions.InsertOneAsync(userReaction);
            return userReaction;
        }

        public async Task<IEnumerable<GroupedUserReaction>> GroupUserReactionByUserIdAsync(string userId)
        {
            var query = _userReactions.AsQueryable()
                                       .Where(x => x.UserId == userId)
                                       .GroupBy(x => new { x.UserId, x.PostId })
                                       .Select(g => new GroupedUserReaction
                                       {
                                           UserId = g.Key.UserId,
                                           PostId = g.Key.PostId,
                                           Count = g.Count()
                                       });

            return await Task.FromResult(query.ToList());
        }
    }
}
