using Chat.Core.Entities;
using Chat.Core.Enums;
using Chat.Core.Repositories;
using Chat.Infrastructure.Persistence.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private IMongoCollection<ChatRoom> _rooms;

        public ChatRepository(IChatContext context)
        {
            _rooms = context.Rooms;
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

        //private FilterDefinition<Message> GetNonDeletedMessageFilter()
        //{
        //    return Builders<Message>.Filter.Eq(x => x.IsDeleted, false);
        //}

        //private FilterDefinition<ChatRoom> GetNonDeletedChatRoomFilter()
        //{
        //    return Builders<ChatRoom>.Filter.Eq(x => x.IsDeleted, false);
        //}

        //private FilterDefinition<ChatParticipant> GetNonDeletedChatParticipantFilter()
        //{
        //    return Builders<ChatParticipant>.Filter.Eq(x => x.IsDeleted, false);
        //}

        //public async Task<ChatRoom> CreateRoomAsync(ChatRoom room, CancellationToken cancellationToken = default)
        //{
        //    await _context.Rooms.InsertOneAsync(room, cancellationToken: cancellationToken);
        //    return room;
        //}

        //public async Task<ChatRoom> GetRoomByIdAsync(string roomId, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<ChatRoom>.Filter.Eq(x => x.Id, roomId) & GetNonDeletedChatRoomFilter();
        //    return await _context.Rooms
        //        .Find(filter)
        //        .FirstOrDefaultAsync(cancellationToken);
        //}

        //public async Task<IEnumerable<string>> GetRoomIdsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<ChatParticipant>.Filter.Eq(x => x.UserId, userId) & GetNonDeletedChatParticipantFilter();
        //    return await _context.Participants
        //        .Find(filter)
        //        .Project(p => p.RoomId)
        //        .ToListAsync(cancellationToken);
        //}

        //public async Task<Message> SaveMessageAsync(Message message, CancellationToken cancellationToken = default)
        //{
        //    await _context.Messages.InsertOneAsync(message, cancellationToken: cancellationToken);
        //    return message;
        //}

        //public async Task<IEnumerable<Message>> GetRoomMessagesAsync(string roomId, int skip, int take, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<Message>.Filter.Eq(m => m.RoomId, roomId) & GetNonDeletedMessageFilter();
        //    return await _context.Messages
        //        .Find(filter)
        //        .SortByDescending(m => m.CreatedAt)
        //        .Skip(skip)
        //        .Limit(take)
        //        .ToListAsync(cancellationToken);
        //}

        //public async Task UpdateMessageStatusAsync(string messageId, MessageStatus status, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<Message>.Filter.Eq(x => x.Id, messageId) & GetNonDeletedMessageFilter();
        //    var update = Builders<Message>.Update.Set(m => m.Status, status);
        //    await _context.Messages.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        //}

        //public async Task<bool> IsUserInRoomAsync(string userId, string roomId, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<ChatParticipant>.Filter.And(
        //        Builders<ChatParticipant>.Filter.Eq(p => p.UserId, userId),
        //        Builders<ChatParticipant>.Filter.Eq(p => p.RoomId, roomId)
        //        );
        //    return await _context.Participants.Find(filter).AnyAsync(cancellationToken);
        //}

        //public async Task<bool> AddUserToRoomAsync(string roomId, string userId, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<ChatRoom>.Filter.Eq(x => x.Id, roomId) & GetNonDeletedChatRoomFilter();

        //    string participantsField = nameof(ChatRoom.ParticipantIds);
        //    var update = Builders<ChatRoom>.Update.AddToSet(participantsField, userId);
        //    var result = await _context.Rooms.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        //    return result.ModifiedCount > 0;
        //}

        //public async Task<bool> RemoveUserFromRoomAsync(string roomId, string userId, CancellationToken cancellationToken = default)
        //{
        //    var filter = Builders<ChatRoom>.Filter.Eq(x => x.Id, roomId) & GetNonDeletedChatRoomFilter();
        //    var update = Builders<ChatRoom>.Update.PullFilter(r => r.ParticipantIds, userId);
        //    var result = await _context.Rooms.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        //    return result.ModifiedCount > 0;
        //}
    }
}
