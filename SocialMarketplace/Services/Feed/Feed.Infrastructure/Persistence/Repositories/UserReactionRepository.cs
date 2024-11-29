using Feed.Core.Entities;
using Feed.Core.Repositories;
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
    }
}
