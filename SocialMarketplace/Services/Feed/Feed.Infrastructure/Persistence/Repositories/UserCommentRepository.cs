using Feed.Core.Entities;
using Feed.Core.Repositories;
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
    }
}
