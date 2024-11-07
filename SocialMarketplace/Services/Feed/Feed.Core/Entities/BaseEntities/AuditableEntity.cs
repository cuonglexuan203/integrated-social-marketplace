using Feed.Core.Entities.AuditProperties;

namespace Feed.Core.Entities.BaseEntities
{
    public abstract class AuditableEntity : ICreatedAt, IModifiedAt, IDeleted
    {
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
