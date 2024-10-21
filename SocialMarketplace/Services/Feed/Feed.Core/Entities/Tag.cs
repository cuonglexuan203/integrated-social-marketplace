using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.BaseEntities;

namespace Feed.Core.Entities
{
    public class Tag : BaseEntity, ICreatedAt, IUpdatedAt, IDeleted
    {
        public string Name { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
