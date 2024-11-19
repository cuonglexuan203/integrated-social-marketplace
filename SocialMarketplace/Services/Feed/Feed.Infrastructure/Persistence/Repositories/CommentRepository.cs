
using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using Feed.Infrastructure.Persistence.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoCollection<Comment> _comments;

        public CommentRepository(IFeedContext context)
        {
            _comments = context.Comments;
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
                ? sort.Descending(x => x.CreatedAt)
                : sort.Ascending(x => x.CreatedAt);
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
    }
}
