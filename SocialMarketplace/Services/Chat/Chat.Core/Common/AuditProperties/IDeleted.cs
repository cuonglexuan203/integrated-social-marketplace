namespace Chat.Core.Common.AuditProperties
{
    public interface IDeleted
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
