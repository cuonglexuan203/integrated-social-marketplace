using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities.BaseEntities
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
