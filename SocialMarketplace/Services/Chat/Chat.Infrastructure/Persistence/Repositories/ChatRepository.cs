using Chat.Core.Entities;
using Chat.Core.Enums;
using Chat.Core.Repositories;
using Chat.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IChatContext _context;

        public ChatRepository(IChatContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom> CreateRoomAsync(ChatRoom room)
        {
            await _context.Rooms.InsertOneAsync(room);
            return room;
        }

        public async Task<ChatRoom> GetRoomByIdAsync(string roomId)
        {
            return await _context.Rooms
                .Find(r => r.Id == roomId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChatRoom>> GetUserRoomsAsync(string userId)
        {
            return await _context.Rooms
                .Find(r => r.Participants.Any(p => p.UserId == userId))
                .ToListAsync();
        }

        public async Task<Message> SaveMessageAsync(Message message)
        {
            await _context.Messages.InsertOneAsync(message);
            return message;
        }

        public async Task<List<Message>> GetRoomMessagesAsync(string roomId, int skip, int take)
        {
            return await _context.Messages
                .Find(m => m.RoomId == roomId)
                .SortByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        public async Task UpdateMessageStatusAsync(string messageId, MessageStatus status)
        {
            var update = Builders<Message>.Update.Set(m => m.Status, status);
            await _context.Messages.UpdateOneAsync(m => m.Id == messageId, update);
        }

        public async Task<bool> IsUserInRoomAsync(string userId, string roomId)
        {
            var room = await GetRoomByIdAsync(roomId);
            return room?.Participants.Any(p => p.UserId == userId) ?? false;
        }
    }
}
