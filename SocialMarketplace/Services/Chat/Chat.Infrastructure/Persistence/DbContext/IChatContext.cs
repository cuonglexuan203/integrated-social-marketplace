
using Chat.Core.Entities;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.DbContext
{
    public interface IChatContext
    {
        IMongoCollection<ChatRoom> Rooms { get; }
        IMongoCollection<Message> Messages { get; }
    }
}
