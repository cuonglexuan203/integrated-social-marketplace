using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities.AuditProperties
{
    public interface IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
