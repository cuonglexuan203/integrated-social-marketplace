using Feed.Core.Entities;
using Feed.Core.Entities.JoinEntities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Feed.Infrastructure.Data
{
    public class FeedContext : IFeedContext
    {
        public IMongoCollection<Comment> Comments { get; }

        public IMongoCollection<Post> Posts { get; }

        public IMongoCollection<Core.Entities.Tag> Tags { get; }

        public IMongoCollection<CommentLike> CommentLikes { get; }

        public IMongoCollection<PostLike> PostLikes { get; }

        public IMongoCollection<PostTag> PostTags { get; }

        public IMongoCollection<SavedPost> SavedPosts { get; }

        public FeedContext(IConfiguration configuration) {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            #region collection assignment
            Comments = database.GetCollection<Comment>(configuration.GetValue<string>("DatabaseSettings:CommentsCollection"));
            Posts = database.GetCollection<Post>(configuration.GetValue<string>("DatabaseSettings:PostsCollection"));
            Tags = database.GetCollection<Core.Entities.Tag>(configuration.GetValue<string>("DatabaseSettings:TagsCollection"));
            CommentLikes = database.GetCollection<CommentLike>(configuration.GetValue<string>("DatabaseSettings:CommentLikesCollection"));
            PostLikes = database.GetCollection<PostLike>(configuration.GetValue<string>("DatabaseSettings:PostLikesCollection"));
            PostTags = database.GetCollection<PostTag>(configuration.GetValue<string>("DatabaseSettings:PostTagsCollection"));
            SavedPosts = database.GetCollection<SavedPost>(configuration.GetValue<string>("DatabaseSettings:SavedPostsCollection"));
            #endregion

            #region populate seed data
            PostContextSeed.SeedData(Posts);
            //CommentContextSeed.SeedData(Comments);
            #endregion
        }
    }
}
