
using Feed.Core.Common.AuditProperties;

namespace Feed.Core.Interfaces
{
    public interface ISoftDeletable<T> where T : IDeleted
    {
        Task<bool> SoftDeleteAsync(string id, CancellationToken token = default);
    }
}
