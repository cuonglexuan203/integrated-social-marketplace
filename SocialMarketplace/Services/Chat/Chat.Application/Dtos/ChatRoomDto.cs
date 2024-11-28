
using Chat.Core.Enums;
using Chat.Core.Specs;

namespace Chat.Application.Dtos
{
    public class ChatRoomDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RoomType Type { get; set; }
        public ICollection<string> ParticipantIds { get; set; }
        public ICollection<UserDto> Participants { get; set; }
        public Pagination<MessageDto>? MessagePage { get; set; }
    }
}
