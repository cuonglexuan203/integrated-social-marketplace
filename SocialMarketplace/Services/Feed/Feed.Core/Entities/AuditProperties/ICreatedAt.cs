namespace Feed.Core.Entities.AuditProperties
{
    public interface ICreatedAt
    {
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
