using Chat.Core.Entities;
using Chat.Core.Enums;

namespace Chat.Core.Repositories
{
    public interface IChatRepository
    {
        Task<ChatRoom> CreateRoomAsync(ChatRoom room);
        Task<ChatRoom> GetRoomByIdAsync(string roomId);
        Task<List<ChatRoom>> GetUserRoomsAsync(string userId);
        Task<Message> SaveMessageAsync(Message message);
        Task<List<Message>> GetRoomMessagesAsync(string roomId, int skip, int take);
        Task UpdateMessageStatusAsync(string messageId, MessageStatus status);
        Task<bool> IsUserInRoomAsync(string userId, string roomId);
    }
}
