using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.Entities.AuditProperties;
using Feed.Core.ValueObjects;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities
{
    public class Post: AuditableEntity, IIdentifier
    {
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; }
        public string ContentText { get; set; }
        public List<Media> Media { get; set; }
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; }
        public int CommentsCount { get; set; }
        public List<Comment> Comments { get; set; }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public List<string> Tags { get; set; }
    }
}
