using Chat.Core.Entities;

namespace Chat.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatRoom> GetOrCreateRoomAsync(string userId, string targetUserId, CancellationToken token = default);
    }
}
