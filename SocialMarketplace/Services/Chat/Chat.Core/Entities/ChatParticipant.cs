using Chat.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.Entities
{
    public class ChatParticipant
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; } // user id
        public ParticipantRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LastReadAt { get; set; }
    }
}
