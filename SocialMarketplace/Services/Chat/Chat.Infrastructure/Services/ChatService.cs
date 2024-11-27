using Chat.Application.Interfaces.Services;
using Chat.Core.Common.Constants;
using Chat.Core.Entities;
using Chat.Core.Specs;
using Chat.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Chat.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly IMongoCollection<ChatRoom> _rooms;
        private readonly IMongoCollection<Message> _messages;

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

        public async Task<Pagination<Message>> GetMessageHistoryAsync(string roomId, MessageSpecParams messageParams, CancellationToken cancellationToken = default)
        {
            var sort = messageParams.Sort?.ToLower() == SortConstants.Ascending ? Builders<Message>.Sort.Ascending(m => m.CreatedAt) :
                Builders<Message>.Sort.Descending(m => m.CreatedAt);

            var dataTask = _messages.Find(m => m.RoomId == roomId)
                                  .Sort(sort)
                                  .Skip((messageParams.PageIndex - 1) * messageParams.PageSize)
                                  .Limit(messageParams.PageSize)
                                  .ToListAsync(cancellationToken: cancellationToken);

            var countTask = _messages.CountDocumentsAsync(m => m.RoomId == roomId, cancellationToken: cancellationToken);

            await Task.WhenAll(dataTask, countTask);

            return new Pagination<Message>(messageParams.PageIndex, messageParams.PageSize, countTask.Result, dataTask.Result);
        }
    }
}
