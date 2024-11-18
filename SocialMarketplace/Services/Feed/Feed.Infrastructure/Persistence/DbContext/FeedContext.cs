using Feed.Core.Entities;
using Feed.Infrastructure.Persistence.SeedData;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.DbContext
{
    public class FeedContext : IFeedContext
    {
        public IMongoCollection<Comment> Comments { get; }
        public IMongoCollection<Post> Posts { get; }
        public FeedContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            #region collection assignment
            Comments = database.GetCollection<Comment>(configuration.GetValue<string>("DatabaseSettings:CommentsCollection"));
            Posts = database.GetCollection<Post>(configuration.GetValue<string>("DatabaseSettings:PostsCollection"));
            #endregion

            #region populate seed data
            CommentContextSeed.SeedData(Comments);
            PostContextSeed.SeedData(Posts);
            #endregion
        }
    }
}
