using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.ValueObjects;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(IFeedContext context, ICommentRepository commentRepository, ILogger<PostRepository> logger)
        {
            _posts = context.Posts;
            _commentRepository = commentRepository;
            _logger = logger;
        }
        public async Task<Post> CreatePost(Post post)
        {
            await _posts.InsertOneAsync(post);
            return post;
        }

        public async Task<bool> DeletePost(string id)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.Id, id);
            var result = await _posts
                .DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Post> GetPost(string id)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.Id, id);
            return await _posts
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<Post>> GetPostByUserId(string userId)
        //{
        //    FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.UserID, userId);
        //    return await _context
        //        .Posts
        //        .Find(filter)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _posts
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.Id, post.Id);
            var result = await _posts
                .ReplaceOneAsync(filter, post);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<Comment> AddCommentToPostAsync(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.PostId))
                throw new BadRequestException("PostId cannot be empty.");

            if (comment is null)
                throw new BadRequestException("Comment cannot be null.");

            try
            {
                if(!(await IsPostExistsAsync(comment.PostId))) 
                    throw new PostNotFoundException(comment.PostId);

                var addedComment = await _commentRepository.CreateComment(comment);

                var filter = Builders<Post>.Filter.Eq(x => x.Id, comment.PostId);
                var updateDef = Builders<Post>.Update.Push(x => x.CommentIds, addedComment.Id);
                await _posts.UpdateOneAsync(filter, updateDef);

                return comment;
            }
            catch (PostNotFoundException)
            {
                _logger.LogWarning($"Cannot add comment to non-existent post: {comment.PostId}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding comment to post");
                throw;
            }
        }

        public async Task<bool> IsPostExistsAsync(string id)
        {
            return await _posts.Find(x => x.Id == id).AnyAsync();
        }

        public async Task<Reaction> AddReacionToPostAsync(string postId, Reaction reaction, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(postId, nameof(postId));
            ArgumentNullException.ThrowIfNull(reaction, nameof(reaction));

            try
            {
                // Check if user already reacted
                var filter = Builders<Post>.Filter.And(
                    Builders<Post>.Filter.Eq(x => x.Id, postId),
                    Builders<Post>.Filter.ElemMatch(x => x.Reactions,
                        r => r.User.Id == reaction.User.Id)
                );

                var existingPost = await _posts.Find(filter)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingPost != null)
                {
                    _logger.LogWarning(
                        "User {UserId} has already reacted to post {PostId}",
                        reaction.User.Id,
                        postId);
                    throw new BadRequestException("User has already reacted to this post");
                }

                // Add new reaction
                var updateFilter = Builders<Post>.Filter.Eq(x => x.Id, postId);
                var updateDef = Builders<Post>.Update.Push(x => x.Reactions, reaction);

                var result = await _posts.UpdateOneAsync(
                    updateFilter,
                    updateDef,
                    new UpdateOptions { IsUpsert = false },
                    cancellationToken);

                if (result.MatchedCount == 0)
                {
                    _logger.LogWarning("Cannot add reaction to non-existent post: {PostId}", postId);
                    throw new PostNotFoundException(postId);
                }

                if (result.ModifiedCount == 0)
                {
                    _logger.LogError("Failed to add reaction to post: {PostId}", postId);
                    throw new DatabaseException($"Failed to add reaction to post {postId}");
                }

                return reaction;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "MongoDB error occurred while adding reaction to post: {PostId}", postId);
                throw new DatabaseException("Failed to add reaction to database", ex);
            }
        }

        public async Task<bool> RemoveReactionFromPostAsync(string postId, string userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
            var update = Builders<Post>.Update.PullFilter(p => p.Reactions, r => r.User.Id == userId);

            var result = await _posts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            if(result.ModifiedCount == 0)
            {
                _logger.LogError("No reaction removed for post id {postId}, user id {userId}. Possible reasons: post not found or reaction does not exist.", postId, userId);
                throw new NotFoundException("Reaction not found or already removed");
            }

            return true;
        }

        public async Task<IEnumerable<Reaction>> GetAllReactionsByPostId(string postId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(postId))
                throw new BadRequestException("PostId cannot be empty.");

            var filter = Builders<Post>.Filter.Eq(x => x.Id, postId);
            var reactions = await _posts.Find(filter)
                .Project(x => x.Reactions)
                .FirstOrDefaultAsync(cancellationToken);

            if (reactions == null)
            {
                _logger.LogWarning("Cannot get reactions of non-existent post: {PostId}", postId);
                throw new PostNotFoundException(postId);
            }

            return reactions;
        }
    }
}
