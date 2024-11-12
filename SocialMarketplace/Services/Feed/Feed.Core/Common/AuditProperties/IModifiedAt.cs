namespace Feed.Core.Common.AuditProperties
{
    public interface IModifiedAt
    {
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
