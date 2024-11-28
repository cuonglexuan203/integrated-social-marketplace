using Chat.Core.Common.AuditProperties;

namespace Chat.Core.Common.BaseEntities
{
    public abstract class AuditableEntity : ICreatedAt, IModifiedAt, IDeleted
    {
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ModifiedAt { get; set; } = DateTimeOffset.Now;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
