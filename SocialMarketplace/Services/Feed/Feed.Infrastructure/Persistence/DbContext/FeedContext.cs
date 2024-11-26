using Feed.Core.Entities;
using Feed.Infrastructure.Configurations;
using Feed.Infrastructure.Persistence.SeedData;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.DbContext
{
    public class FeedContext : IFeedContext
    {
        public IMongoCollection<Comment> Comments { get; }
        public IMongoCollection<Post> Posts { get; }
        public IMongoCollection<SavedPost> SavedPosts { get; }

        public FeedContext(IOptions<DatabaseSettings> dbOptions)
        {
            var client = new MongoClient(dbOptions.Value.ConnectionString);
            var database = client.GetDatabase(dbOptions.Value.DatabaseName);

            #region collection assignment
            Comments = database.GetCollection<Comment>(dbOptions.Value.CommentsCollection);
            Posts = database.GetCollection<Post>(dbOptions.Value.PostsCollection);
            SavedPosts = database.GetCollection<SavedPost>(dbOptions.Value.SavedPostsCollection);
            #endregion

            #region populate seed data
            CommentContextSeed.SeedData(Comments);
            PostContextSeed.SeedData(Posts);
            #endregion
        }
    }
}
