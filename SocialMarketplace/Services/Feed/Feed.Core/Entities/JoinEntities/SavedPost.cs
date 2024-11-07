using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities.JoinEntities
{
    public class SavedPost : BaseEntity, ICreatedAt, IModifiedAt, IDeleted
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
