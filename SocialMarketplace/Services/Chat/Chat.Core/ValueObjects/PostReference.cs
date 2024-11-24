using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.ValueObjects
{
    public class PostReference
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PostId { get; set; }
        public string ContentText { get; set; }
        public string Link { get; set; }
        public string ThunbnailUrl { get; set; }
    }
}
