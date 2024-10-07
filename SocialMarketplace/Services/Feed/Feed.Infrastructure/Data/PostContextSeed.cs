using Feed.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Feed.Infrastructure.Data
{
    public static class PostContextSeed
    {
        public static void SeedData(IMongoCollection<Post> postCollection)
        {
            bool checkPost = postCollection.Find(p => true).Any();
            string path = Path.Combine("Data", "SeedData", "posts.json");
            if (!checkPost)
            {
                var postData = File.ReadAllText(path);
                var posts = JsonSerializer.Deserialize<List<Post>>(postData);
                if (posts != null)
                {
                    postCollection.InsertMany(posts);
                }
            }
        }

    }
}
