using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities
{
    public class PostLike : BaseEntity, ICreatedAt
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
    }

}
