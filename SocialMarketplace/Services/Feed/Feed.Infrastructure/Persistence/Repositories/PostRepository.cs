using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IFeedContext _context;

        public PostRepository(IFeedContext context)
        {
            _context = context;
        }
        public async Task<Post> CreatePost(Post post)
        {
            await _context.Posts.InsertOneAsync(post);
            return post;
        }

        public async Task<bool> DeletePost(string id)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.Id, id);
            var result = await _context
                .Posts
                .DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Post> GetPost(string id)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.Id, id);
            return await _context
                .Posts
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
            return await _context
                .Posts
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Eq(p => p.Id, post.Id);
            var result = await _context
                .Posts
                .ReplaceOneAsync(filter, post);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> AddCommentToPostAsync(string postId, Comment comment)
        {
            if (string.IsNullOrWhiteSpace(postId) || comment == null)
            {
                throw new ArgumentException("Invalid postId or comment.");
            }

            try
            {
                var filter = Builders<Post>.Filter.Eq(x => x.Id, postId);
                var updateDef = Builders<Post>.Update.Push(x => x.Comments, comment);

                var result = await _context.Posts.UpdateOneAsync(filter, updateDef);

                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
