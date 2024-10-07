using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Feed.Core.Entities
{
    public class SharedPost : Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string SharedPostId { get; set; }
    }
}
