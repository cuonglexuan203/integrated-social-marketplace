using Feed.Core.Common.AuditProperties;

namespace Feed.Core.Common.BaseEntities
{
    public abstract class AuditableEntity : ICreatedAt, IModifiedAt, IDeleted
    {
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
