using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities
{
    public class Comment : BaseEntity, ICreatedAt, IUpdatedAt, IDeleted
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; }
        public List<string> MediaURL { get; set; }

        public string CommentText { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentCommentID { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }

}
