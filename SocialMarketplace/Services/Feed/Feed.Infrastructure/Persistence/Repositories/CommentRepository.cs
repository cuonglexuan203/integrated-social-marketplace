
using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using Feed.Core.ValueObjects;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoCollection<Comment> _comments;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(IFeedContext context, ILogger<CommentRepository> logger)
        {
            _comments = context.Comments;
            _logger = logger;
        }
        public async Task<Comment> CreateComment(Comment comment)
        {
            await _comments.InsertOneAsync(comment);
            return comment;
        }

        public async Task<bool> DeleteComment(string id)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.Id, id);
            var result = await _comments.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Comment> GetComment(string id)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.Id, id);
            return await _comments.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByPostId(string postId)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.PostId, postId);
            var result = await _comments.Find(filter).ToListAsync();
            return result;
        }

        private FilterDefinition<Comment> BuildFilter(CommentSpecParams commentParams, string postId)
        {
            var builder = Builders<Comment>.Filter;
            var filter = builder.Empty;

            filter &= builder.Eq(x => x.PostId, postId);

            if (!string.IsNullOrEmpty(commentParams.Search))
            {
                var searchFilter = builder.Regex(x => x.CommentText, new BsonRegularExpression(commentParams.Search));
                filter &= searchFilter;
            }
            return filter;
        }

        private SortDefinition<Comment> BuildSort(CommentSpecParams commentParams)
        {
            var sort = Builders<Comment>.Sort;
            return commentParams.Sort?.ToLower() == "desc"
                ? sort.Descending(x => x.ModifiedAt)
                : sort.Ascending(x => x.ModifiedAt);
        }

        public async Task<Pagination<Comment>> GetCommentsByPostId(string postId, CommentSpecParams commentParams)
        {
            if(string.IsNullOrEmpty(postId))
                throw new BadRequestException("PostId cannot be empty.");

            var filter = BuildFilter(commentParams, postId);
            var sort = BuildSort(commentParams);

            var countTask = _comments.CountDocumentsAsync(filter);
            var dataTask = _comments
                                .Find(filter)
                                .Sort(sort)
                                .Skip((commentParams.PageIndex - 1) * commentParams.PageSize)
                                .Limit(commentParams.PageSize)
                                .ToListAsync();

            await Task.WhenAll(dataTask, countTask);

            return new Pagination<Comment>()
            {
                PageIndex = commentParams.PageIndex,
                PageSize = commentParams.PageSize,
                Data = dataTask.Result,
                Count = countTask.Result,
            };
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.Id, comment.Id);
            var result = await _comments.ReplaceOneAsync(filter, comment);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _comments.Find(_ => true).ToListAsync();
        }

        public async Task<Reaction> AddReacionToCommentAsync(string commentId, Reaction reaction, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(commentId, nameof(commentId));
            ArgumentNullException.ThrowIfNull(reaction, nameof(reaction));

            try
            {
                // Check if user already reacted
                var filter = Builders<Comment>.Filter.And(
                    Builders<Comment>.Filter.Eq(x => x.Id, commentId),
                    Builders<Comment>.Filter.ElemMatch(x => x.Reactions,
                        r => r.User.Id == reaction.User.Id)
                );

                var existingPost = await _comments.Find(filter)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingPost != null)
                {
                    _logger.LogWarning(
                        "User {UserId} has already reacted to comment {PostId}",
                        reaction.User.Id,
                        commentId);
                    throw new BadRequestException("User has already reacted to this post");
                }

                // Add new reaction
                var updateFilter = Builders<Comment>.Filter.Eq(x => x.Id, commentId);
                var updateDef = Builders<Comment>.Update.Push(x => x.Reactions, reaction);

                var result = await _comments.UpdateOneAsync(
                    updateFilter,
                    updateDef,
                    new UpdateOptions { IsUpsert = false },
                    cancellationToken);

                if (result.MatchedCount == 0)
                {
                    _logger.LogError("Cannot add reaction to non-existent comment: {commentId}", commentId);
                    throw new CommentNotFoundException(commentId);
                }

                if (result.ModifiedCount == 0)
                {
                    _logger.LogError("Failed to add reaction to comment: {commentId}", commentId);
                    throw new DatabaseException($"Failed to add reaction to comment {commentId}");
                }

                return reaction;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "MongoDB error occurred while adding reaction to post: {commentId}", commentId);
                throw new DatabaseException("Failed to add reaction to database", ex);
            }
        }

        public async Task<bool> RemoveReactionFromCommentAsync(string commentId, string userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Comment>.Filter.Eq(p => p.Id, commentId);
            var update = Builders<Comment>.Update.PullFilter(p => p.Reactions, r => r.User.Id == userId);

            var result = await _comments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            if (result.ModifiedCount == 0)
            {
                _logger.LogError("No reaction removed for comment id {commentId}, user id {userId}. Possible reasons: comment not found or reaction does not exist.", commentId, userId);
                throw new NotFoundException("Reaction not found or already removed");
            }

            return true;
        }
    }
}
