using Feed.Core.Entities;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.DbContext
{
    public interface IFeedContext
    {
        IMongoCollection<Comment> Comments { get; }
        IMongoCollection<Post> Posts { get; }
        IMongoCollection<SavedPost> SavedPosts { get; }
        IMongoCollection<Report> Reports { get; }
        IMongoCollection<UserComment> UserComments { get; }
        IMongoCollection<UserReaction> UserReactions { get; }
        IMongoCollection<UserShare> UserShares { get; }
    }
}
