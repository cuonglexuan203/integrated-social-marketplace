using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Core.ValueObjects;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class UserCommentRepository : IUserCommentsRepository
    {
        private readonly ILogger<UserCommentRepository> _logger;
        private readonly IMongoCollection<UserComment> _userComments;

        public UserCommentRepository(ILogger<UserCommentRepository> logger, IFeedContext feedContext)
        {
            _logger = logger;
            _userComments = feedContext.UserComments;
        }
        public async Task<long> CountTotalCommentsByPostIdAsync(string postId)
        {
            return await _userComments.CountDocumentsAsync(x => x.PostId == postId);
        }

        public async Task<long> CountTotalCommentsByUserIdAsync(string userId)
        {
            return await _userComments.CountDocumentsAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<UserComment>> GetUserCommentsByPostId(string postId)
        {
            return await _userComments.Find(x => x.PostId == postId).ToListAsync();
        }
        public async Task<UserComment> CreateUserCommentAsync(UserComment userComment)
        {
            await _userComments.InsertOneAsync(userComment);
            return userComment;
        }
        public async Task<IEnumerable<GroupedUserComment>> GroupUserCommentByUserIdAsync(string userId)
        {
            var query = _userComments.AsQueryable()
                                       .Where(x => x.UserId == userId)
                                       .GroupBy(x => new { x.UserId, x.PostId })
                                       .Select(g => new GroupedUserComment
                                       {
                                           UserId = g.Key.UserId,
                                           PostId = g.Key.PostId,
                                           TotalSentimentScore = g.Sum(x => x.SentimentScore),
                                           Count = g.Count()
                                       });

            return await Task.FromResult(query.ToList());
        }

    }
}
