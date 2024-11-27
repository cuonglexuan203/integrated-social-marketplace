using Chat.Core.Entities;
using Chat.Core.Specs;

namespace Chat.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatRoom> GetOrCreateRoomAsync(string userId, string targetUserId, CancellationToken token = default);
        Task<Pagination<Message>> GetMessageHistoryAsync(string roomId, MessageSpecParams messageParams, CancellationToken cancellationToken = default);
    }
}
