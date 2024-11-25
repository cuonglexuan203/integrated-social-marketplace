using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Feed.Core.Common.AuditProperties;

namespace Feed.Core.Entities
{
    public class SavedPost : IIdentifier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public DateTimeOffset? SavedAt { get; set; } = DateTimeOffset.Now;
    }
}
