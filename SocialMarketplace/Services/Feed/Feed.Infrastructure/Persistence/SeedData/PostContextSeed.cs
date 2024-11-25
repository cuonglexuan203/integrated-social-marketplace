using Feed.Application.DTOs;
using Feed.Core.Entities;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Feed.Infrastructure.Persistence.SeedData
{
    public static class PostContextSeed
    {
        public static void SeedData(IMongoCollection<Post> postCollection)
        {
            bool checkPost = postCollection.Find(p => true).Any();
            string path = Path.Combine("Persistence", "SeedData", "Data", "posts.json");
            if (!checkPost)
            {
                // run local
                var postData = File.ReadAllText("../Feed.Infrastructure/Persistence/SeedData/Data/posts.json");
                // run by docker file
                //var postData = File.ReadAllText("../src/services/Feed/Feed.Infrastructure/Persistence/SeedData/Data/posts.json");
                // run by docker compose
                //var postData = File.ReadAllText(path);
                var posts = JsonConvert.DeserializeObject<List<Post>>(postData);
                if (posts != null && posts?.Count > 0)
                {
                    postCollection.InsertMany(posts);
                }
            }
        }

    }
}
