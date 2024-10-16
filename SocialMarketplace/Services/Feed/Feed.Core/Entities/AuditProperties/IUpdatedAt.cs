namespace Feed.Core.Entities.AuditProperties
{
    public interface IUpdatedAt
    {
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
