using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Feed.Core.Entities
{
    public class Post : AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("User")]
        [JsonPropertyName("user")]
        [JsonProperty("user")]
        public CompactUser CompactUser { get; set; }
        public string ContentText { get; set; }
        public List<Media> Media { get; set; } = new List<Media>();
        public long TotalReactions { get; private set; }
        private List<Reaction> _reactions = new List<Reaction>();
        public List<Reaction> Reactions
        {
            get => _reactions;
            set
            {
                _reactions = value;
                TotalReactions = value.Count;
            }
        }
        public long TotalComments { get; private set; }
        private List<string> _commentIds { get; set; } = new List<string>();
        public List<string> CommentIds
        {
            get => _commentIds; set
            {
                _commentIds = value;
                TotalComments = value.Count;
            }
        }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
