﻿using Feed.Core.Entities;
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
        public IMongoCollection<Report> Reports { get; }
        public IMongoCollection<UserComment> UserComments { get; }
        public IMongoCollection<UserReaction> UserReactions { get; }
        public IMongoCollection<UserShare> UserShares { get; }
        public FeedContext(IOptions<DatabaseSettings> dbOptions)
        {
            var client = new MongoClient(dbOptions.Value.ConnectionString);
            var database = client.GetDatabase(dbOptions.Value.DatabaseName);

            #region collection assignment
            Comments = database.GetCollection<Comment>(dbOptions.Value.CommentsCollection);
            Posts = database.GetCollection<Post>(dbOptions.Value.PostsCollection);
            SavedPosts = database.GetCollection<SavedPost>(dbOptions.Value.SavedPostsCollection);
            Reports = database.GetCollection<Report>(dbOptions.Value.ReportsCollection);
            UserComments = database.GetCollection<UserComment>(dbOptions.Value.UserCommentsCollection);
            UserReactions = database.GetCollection<UserReaction>(dbOptions.Value.UserReactionsCollection);
            UserShares = database.GetCollection<UserShare>(dbOptions.Value.UserSharesCollection);
            #endregion
        }

        public async Task InitializeAsync()
        {
            await CreatePostContentTextIndexAsync();
        }

        public async Task CreatePostContentTextIndexAsync()
        {
            var indexes = await Posts.Indexes.ListAsync();
            var existingIndexes = await indexes.ToListAsync();

            bool indexExists = existingIndexes.Any(index =>
            {
                var definition = index["key"].AsBsonDocument;
                return definition.Contains(nameof(Post.ContentText)) && definition[nameof(Post.ContentText)].ToString() == "text";
            });

            if (!indexExists)
            {
                var indexKeys = Builders<Post>.IndexKeys.Text(p => p.ContentText);
                var indexModel = new CreateIndexModel<Post>(indexKeys);
                await Posts.Indexes.CreateOneAsync(indexModel);
            }
        }
    }
}
