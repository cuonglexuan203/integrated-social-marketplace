using Chat.Core.Entities;
using Chat.Core.Specs;

namespace Chat.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatRoom> GetOrCreateRoomAsync(string userId, string targetUserId, CancellationToken token = default);
        Task<Pagination<Message>> GetMessageHistoryAsync(string roomId, MessageSpecParams messageParams, CancellationToken cancellationToken = default);
        Task<Message> SaveMessageAsync(Message message, CancellationToken cancellationToken = default);
        Task<List<ChatRoom>> GetUserRoomsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
