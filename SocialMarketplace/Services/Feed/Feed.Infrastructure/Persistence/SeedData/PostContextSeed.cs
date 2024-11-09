using Feed.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Feed.Infrastructure.Persistence.SeedData
{
    public static class PostContextSeed
    {
        public static void SeedData(IMongoCollection<Post> postCollection)
        {
            bool checkPost = postCollection.Find(p => true).Any();
            //string path = Path.Combine("Persistence", "SeedData", "Data", "posts.json");
            if (!checkPost)
            {
                // local 1
                var postData = File.ReadAllText("../Feed.Infrastructure/Persistence/SeedData/Data/posts.json");
                // local 2 ( debug mode )
                //var postData = File.ReadAllText("../src/services/Feed/Feed.Infrastructure/Persistence/SeedData/Data/posts.json");
                //
                //var postData = File.ReadAllText(path);
                var posts = JsonSerializer.Deserialize<List<Post>>(postData);
                if (posts != null)
                {
                    postCollection.InsertMany(posts);
                }
            }
        }

    }
}
