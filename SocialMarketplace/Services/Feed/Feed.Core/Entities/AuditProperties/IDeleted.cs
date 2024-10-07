namespace Feed.Core.Entities.AuditProperties
{
    public interface IDeleted
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
