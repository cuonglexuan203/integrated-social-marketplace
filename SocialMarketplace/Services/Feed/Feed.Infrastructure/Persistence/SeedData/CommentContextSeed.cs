using Feed.Core.Entities;
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
                // local (debug)
                var commentData = File.ReadAllText("../Feed.Infrastructure/Persistence/SeedData/Data/comments.json");
                //var commentData = File.ReadAllText(path);
                var comments = JsonConvert.DeserializeObject<List<Comment>>(commentData);
                if (comments != null)
                {
                    commentCollection.InsertMany(comments);
                }
            }
        }
    }
}
