using Feed.Core.Entities;
using Feed.Core.Entities.JoinEntities;
using MongoDB.Driver;

namespace Feed.Infrastructure.Data
{
    public interface IFeedContext
    {
        IMongoCollection<Comment> Comments { get; }
        IMongoCollection<Post> Posts { get; }
        IMongoCollection<Core.Entities.Tag> Tags { get; }
        IMongoCollection<CommentLike> CommentLikes { get; }
        IMongoCollection<PostLike> PostLikes { get; }
        IMongoCollection<PostTag> PostTags { get; }
        IMongoCollection<SavedPost> SavedPosts { get; }
    }
}
