using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class UserShareRepository : IUserShareRepository
    {
        private readonly ILogger<UserShareRepository> _logger;
        private readonly IMongoCollection<UserShare> _userShares;

        public UserShareRepository(ILogger<UserShareRepository> logger, IFeedContext feedContext)
        {
            _logger = logger;
            _userShares = feedContext.UserShares;
        }
        public async Task<long> CountTotalSharesByPostIdAsync(string postId)
        {
            return await _userShares.CountDocumentsAsync(_ => _.PostId == postId);
        }

        public async Task<long> CountTotalSharesByUserIdAsync(string userId)
        {
            return await _userShares.CountDocumentsAsync(_ => _.UserId == userId);
        }

        public async Task<UserShare> CreateUserShareAsync(UserShare userShare)
        {
            await _userShares.InsertOneAsync(userShare);
            return userShare;
        }
    }
}
