using MongoDB.Bson.Serialization.Attributes;
using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed.Core.Entities.JoinEntities
{
    public class PostTag : BaseEntity, ICreatedAt, IModifiedAt, IDeleted
    {

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PostId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string TagId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
