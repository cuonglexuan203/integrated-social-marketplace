
using Chat.Core.Entities;
using Chat.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.DbContext
{
    public class ChatContext: IChatContext
    {
        public virtual IMongoCollection<ChatRoom> Rooms { get; set; }
        public virtual IMongoCollection<Message> Messages { get; set; }
        public ChatContext(IOptions<DatabaseSettings> dbOptions)
        {
            var client = new MongoClient(dbOptions.Value.ConnectionString);
            var db = client.GetDatabase(dbOptions.Value.DatabaseName);

            #region Collection assignment

            Rooms = db.GetCollection<ChatRoom>(dbOptions.Value.ChatRoomCollection);
            Messages = db.GetCollection<Message>(dbOptions.Value.MessageCollection);

            #endregion

            #region Seed data

            #endregion
        }
    }
}
