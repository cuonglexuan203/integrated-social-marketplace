using Chat.Core.Common.AuditProperties;
using Chat.Core.Common.BaseEntities;
using Chat.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.Entities
{
    public class ChatParticipant: AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoomId { get; set; }
        public ParticipantRole Role { get; set; } = ParticipantRole.Member;
        public DateTimeOffset? LastReadAt { get; set; }
    }
}
