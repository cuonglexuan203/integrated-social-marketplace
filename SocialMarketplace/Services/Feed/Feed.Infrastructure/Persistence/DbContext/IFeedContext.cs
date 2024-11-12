using Feed.Core.Entities;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.DbContext
{
    public interface IFeedContext
    {
        IMongoCollection<Comment> Comments { get; }
        IMongoCollection<Post> Posts { get; }
    }
}
