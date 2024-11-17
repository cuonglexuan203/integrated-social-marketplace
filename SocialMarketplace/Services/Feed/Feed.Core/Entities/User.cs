
using Feed.Core.Common.AuditProperties;
using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities
{
    public class User : IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}
