
using Chat.Core.Enums;

namespace Chat.Application.Dtos
{
    public class ChatRoomDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RoomType Type { get; set; }
        public ICollection<UserDto> Participants { get; set; }
    }
}
