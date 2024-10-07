using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities
{
    public class Post: BaseEntity, ICreatedAt, IUpdatedAt, IDeleted
    {
        public string Title { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; }
        public string ContentText { get; set; }
        public List<string> MediaURL { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string Link { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
