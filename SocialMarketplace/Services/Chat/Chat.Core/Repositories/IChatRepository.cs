using Chat.Core.Entities;
using Chat.Core.Enums;

namespace Chat.Core.Repositories
{
    public interface IChatRepository
    {
        Task<ChatRoom> CreateRoomAsync(ChatRoom room, CancellationToken cancellationToken = default);
        Task<ChatRoom> GetRoomByIdAsync(string roomId, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetRoomIdsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> AddUserToRoomAsync(string roomId, string userId, CancellationToken cancellationToken = default);
        Task<bool> RemoveUserFromRoomAsync(string roomId, string userId, CancellationToken cancellationToken = default);
        Task<Message> SaveMessageAsync(Message message, CancellationToken cancellationToken = default);
        Task<IEnumerable<Message>> GetRoomMessagesAsync(string roomId, int skip, int take, CancellationToken cancellationToken = default);
        Task UpdateMessageStatusAsync(string messageId, MessageStatus status, CancellationToken cancellationToken = default);
        Task<bool> IsUserInRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default);
    }
}
