using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities
{
    public class Post : AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public User User { get; set; }
        public string ContentText { get; set; }
        public ICollection<Media> Media { get; set; } = new List<Media>();
        public ICollection<Reaction> Reactions {  get; set; } = new List<Reaction>();
        public ICollection<string> CommentIds { get; set; } = new List<string>();
        public string Link { get => Id; }
        public string? SharedPostId { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
        public float FinalPostScore { get; set; }
    }
}
