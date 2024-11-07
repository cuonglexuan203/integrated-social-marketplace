using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities
{
    public class Tag : BaseEntity, ICreatedAt, IModifiedAt, IDeleted
    {
        public string Name { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
