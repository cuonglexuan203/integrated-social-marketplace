using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Feed.Core.Entities
{
    public class Comment : AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        [BsonElement("User")]
        [JsonPropertyName("user")]
        [JsonProperty("user")]
        public CompactUser CompactUser { get; set; }
        public IEnumerable<Media> Media { get; set; } = new List<Media>();
        public string CommentText { get; set; }
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

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentCommentID { get; set; }
    }

}
