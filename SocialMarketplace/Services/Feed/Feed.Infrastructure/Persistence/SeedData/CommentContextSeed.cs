using Feed.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Feed.Infrastructure.Persistence.SeedData
{
    public static class CommentContextSeed
    {
        public static void SeedData(IMongoCollection<Comment> commentCollection)
        {
            bool checkPost = commentCollection.Find(p => true).Any();
            string path = Path.Combine("Data", "SeedData", "comments.json");
            if (!checkPost)
            {
                var commentData = File.ReadAllText(path);
                var comments = JsonSerializer.Deserialize<List<Comment>>(commentData);
                if (comments != null)
                {
                    commentCollection.InsertMany(comments);
                }
            }
        }
    }
}
