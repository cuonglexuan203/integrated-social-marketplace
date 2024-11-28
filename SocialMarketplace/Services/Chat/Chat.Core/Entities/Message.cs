
using Chat.Core.Common.AuditProperties;
using Chat.Core.Common.BaseEntities;
using Chat.Core.Enums;
using Chat.Core.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Core.Entities
{
    public class Message: AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string SenderId { get; set; } // participant id
        public string ContentText { get; set; }
        public ICollection<Media> Media { get; set; } = new List<Media>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<MessageReadInfo> MessageReadInfo { get; set; } = new List<MessageReadInfo>();
        public ICollection<PostReference> AttachedPosts { get; set; } = new List<PostReference>();
        public MessageStatus Status { get; set; } = MessageStatus.Pending;
    }
}
