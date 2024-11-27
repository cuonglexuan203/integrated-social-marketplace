
using Chat.Core.Enums;

namespace Chat.Application.Dtos
{
    public class ReactionDto
    {
        public UserDto User { get; set; }
        public ReactionType Type { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
