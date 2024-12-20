﻿using Feed.Core.Entities;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Feed.Infrastructure.Persistence.SeedData
{
    public static class CommentContextSeed
    {
        public static void SeedData(IMongoCollection<Comment> commentCollection)
        {
            bool checkPost = commentCollection.Find(p => true).Any();
            string path = Path.Combine("Persistence", "SeedData", "Data", "comments.json");
            if (!checkPost)
            {
                // run local
                //var commentData = File.ReadAllText("../Feed.Infrastructure/Persistence/SeedData/Data/comments.json");
                // run by docker compose
                var commentData = File.ReadAllText(path);
                var comments = JsonConvert.DeserializeObject<List<Comment>>(commentData);
                if (comments != null & comments?.Count > 0)
                {
                    commentCollection.InsertMany(comments);
                }
            }
        }
    }
}
