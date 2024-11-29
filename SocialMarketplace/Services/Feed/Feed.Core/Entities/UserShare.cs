using Feed.Core.Common.AuditProperties;
using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities
{
    public class UserShare : IIdentifier, ICreatedAt
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
    }
}
