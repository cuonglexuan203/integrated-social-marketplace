
using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using Feed.Infrastructure.Persistence.DbContext;
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
        public Task<bool> CreateComment(Comment post)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteComment(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetComment(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetCommentByPostID(string postId)
        {
            throw new NotImplementedException();
        }

        private FilterDefinition<Comment> BuildFilter(CommentSpecParams commentParams)
        {
            var builder = Builders<Comment>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(commentParams.Search))
            {
                var searchFilter = builder.Regex(x => x.CommentText, new MongoDB.Bson.BsonRegularExpression(commentParams.Search));
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

        public async Task<Pagination<Comment>> GetComments(CommentSpecParams commentParams)
        {
            var filter = BuildFilter(commentParams);
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

        public Task<bool> UpdateComment(Comment post)
        {
            throw new NotImplementedException();
        }
    }
}
