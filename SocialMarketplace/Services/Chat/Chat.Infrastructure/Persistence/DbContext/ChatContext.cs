
using Chat.Core.Entities;
using Chat.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.DbContext
{
    public class ChatContext: IChatContext
    {
        public IMongoCollection<ChatRoom> Rooms { get; }
        public IMongoCollection<Message> Messages { get; }
        public ChatContext(IOptions<DatabaseSettings> dbOptions)
        {
            var client = new MongoClient(dbOptions.Value.ConnectionString);
            var db = client.GetDatabase(dbOptions.Value.DatabaseName);

            #region Collection assignment

            Rooms = db.GetCollection<ChatRoom>(dbOptions.Value.ChatRoomCollection);
            Messages = db.GetCollection<Message>(dbOptions.Value.MessageCollection);
            #endregion
        }

        public async Task InitializeAsync()
        {
            await CreateMessageTextIndexAsync();
        }

        public async Task CreateMessageTextIndexAsync()
        {
            var indexes = await Messages.Indexes.ListAsync();
            var existingIndexes = await indexes.ToListAsync();

            bool indexExists = existingIndexes.Any(index =>
            {
                var definition = index["key"].AsBsonDocument;
                return definition.Contains(nameof(Message.ContentText)) && definition[nameof(Message.ContentText)].ToString() == "text";
            });

            if (!indexExists)
            {
                var indexKeys = Builders<Message>.IndexKeys.Text(m => m.ContentText);
                var indexModel = new CreateIndexModel<Message>(indexKeys);
                await Messages.Indexes.CreateOneAsync(indexModel);
            }
        }
    }
}
