using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
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

        public async Task<IEnumerable<Post>> GetAllPosts()
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
    }
}
