using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities
{
    public class Post: AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public CompactUser User { get; set; }
        public string ContentText { get; set; }
        public List<Media> Media { get; set; } = new List<Media>();
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; } = new List<Reaction>();
        public int CommentsCount { get; set; }
        public List<string> CommentIds { get; set; } = new List<string>();
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
