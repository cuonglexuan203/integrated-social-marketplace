using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;

namespace Feed.Core.Entities
{
    public class Comment : AuditableEntity, IIdentifier
    {
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Username { get; set; }
        public List<Media> Media { get; set; }
        public string CommentText { get; set; }
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentCommentID { get; set; }
    }

}
