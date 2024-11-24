namespace Chat.Core.Common.AuditProperties
{
    public interface ICreatedAt
    {
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
