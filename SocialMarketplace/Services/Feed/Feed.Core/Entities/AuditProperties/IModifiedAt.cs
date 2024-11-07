namespace Feed.Core.Entities.AuditProperties
{
    public interface IModifiedAt
    {
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
