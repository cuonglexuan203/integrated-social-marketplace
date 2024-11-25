using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class SavedPostRepository : ISavedPostRepository
    {
        private readonly ILogger<SavedPostRepository> _logger;
        private IMongoCollection<SavedPost> _savedPosts;
        public SavedPostRepository(IFeedContext feedContext, ILogger<SavedPostRepository> logger)
        {
            _savedPosts = feedContext.SavedPosts;
            _logger = logger;
        }

        public async Task<IEnumerable<SavedPost>> GetSavedPostsAsync(string userId, CancellationToken token = default)
        {
            var savedPosts = await _savedPosts
             .Find(sp => sp.UserId == userId)
             .SortByDescending(sp => sp.SavedAt)
             .ToListAsync(token);

            return savedPosts;
        }

        public async Task<bool> RemoveSavedPostAsync(string userId, string postId, CancellationToken token = default)
        {
            try
            {
                var result = await _savedPosts.DeleteOneAsync(sp => sp.UserId == userId && sp.PostId == postId, cancellationToken: token);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while deleting saved post: {errorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<SavedPost> SavePostAsync(string userId, string postId, CancellationToken token = default)
        {
            try
            {
                var existingSave = await _savedPosts.Find(sp => sp.UserId == userId && sp.PostId == postId).FirstOrDefaultAsync();
                if (existingSave != null)
                {
                    _logger.LogError("User has already saved this post: post Id {postId}", postId);
                    throw new BadRequestException("User has already saved this post");
                }

                var savedPost = new SavedPost
                {
                    UserId = userId,
                    PostId = postId,
                };

                await _savedPosts.InsertOneAsync(savedPost, cancellationToken: token);
                return savedPost;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while saving saved post: {errorMessage}", ex.Message);
                throw;
            }
        }
    }
}
