using Chat.Core.Common.AuditProperties;

namespace Chat.Core.Interfaces
{
    public interface ISoftDeletable<T> where T : IDeleted
    {
        Task<bool> SoftDeleteAsync(string id, CancellationToken token = default);
    }
}
