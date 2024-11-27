using Chat.Application.Interfaces.Services;
using Chat.Core.Entities;
using Chat.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Chat.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private IMongoCollection<ChatRoom> _rooms;
        private IMongoCollection<Message> _messages;

        public ChatService(IChatContext chatContext)
        {
            _rooms = chatContext.Rooms;
            _messages = chatContext.Messages;
        }

        public async Task<ChatRoom> GetOrCreateRoomAsync(string userId, string targetUserId, CancellationToken token = default)
        {
            var room = await _rooms.Find(r =>
                r.ParticipantIds.Contains(userId) && r.ParticipantIds.Contains(targetUserId)).FirstOrDefaultAsync();

            if (room == null)
            {
                room = new ChatRoom
                {
                    ParticipantIds = new List<string> { userId, targetUserId }
                };
                await _rooms.InsertOneAsync(room);
            }
            return room;
        }
    }
}
