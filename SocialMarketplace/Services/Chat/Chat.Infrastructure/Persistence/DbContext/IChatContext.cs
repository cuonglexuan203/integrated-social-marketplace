
using Chat.Core.Entities;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.DbContext
{
    public interface IChatContext
    {
        Task InitializeAsync();
        IMongoCollection<ChatRoom> Rooms { get; }
        IMongoCollection<Message> Messages { get; }
    }
}
