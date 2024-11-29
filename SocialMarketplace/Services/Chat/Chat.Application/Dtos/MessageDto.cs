using Chat.Core.Enums;

namespace Chat.Application.Dtos
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string SenderId { get; set; }
        public string ContentText { get; set; }
        public ICollection<MediaDto> Media { get; set; }
        public ICollection<ReactionDto> Reactions { get; set; }
        public ICollection<MessageReadInfoDto> MessageReadInfo { get; set; }
        public ICollection<PostReferenceDto> AttachedPosts { get; set; }
        public MessageStatus Status { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
