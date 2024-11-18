using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;

namespace Feed.Core.Entities
{
    public class Comment : AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public CompactUser User { get; set; }
        public List<Media> Media { get; set; } = new List<Media>();
        public string CommentText { get; set; }
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; } = new List<Reaction>();

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentCommentID { get; set; }
    }

}
