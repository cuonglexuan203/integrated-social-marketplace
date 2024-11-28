
using Chat.Application.Dtos;
using Chat.Core.Entities;

namespace Chat.Application.Interfaces.Services
{
    public interface IChatRoomMappingService
    {
        Task<ChatRoomDto> MapToDtoAsync(ChatRoom room, CancellationToken cancellationToken = default);
    }
}
