using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.ValueObjects
{
    public class PostReference
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } // post id
        public string ContentText { get; set; }
        public string Link { get; set; }
        public ICollection<Media> Media { get; set; } = new List<Media>();
    }
}
