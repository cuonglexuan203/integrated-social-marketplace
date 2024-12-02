using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Core.ValueObjects;
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
        public async Task<IEnumerable<GroupedUserShare>> GroupUserShareByUserIdAsync(string userId)
        {
            var query = _userShares.AsQueryable()
                                       .GroupBy(x => new { x.UserId, x.PostId })
                                       .Select(g => new GroupedUserShare
                                       {
                                           UserId = g.Key.UserId,
                                           PostId = g.Key.PostId,
                                           Count = g.Count()
                                       });

            return await Task.FromResult(query.ToList());
        }
    }
}
