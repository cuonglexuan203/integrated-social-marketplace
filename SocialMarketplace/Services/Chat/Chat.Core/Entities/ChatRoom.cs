using Chat.Core.Common.AuditProperties;
using Chat.Core.Common.BaseEntities;
using Chat.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.Entities
{
    public class ChatRoom : AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public RoomType Type { get; set; } = RoomType.OneToOne;
        public ICollection<string> ParticipantIds { get; set; } = new List<string>();
    }
}
