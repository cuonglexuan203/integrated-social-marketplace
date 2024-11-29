
using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface IReportRepository
    {
        Task<long> CountReportsByUserIdAsync(string userId);
        Task<long> CountReportsByTargetUserIdAsync(string userId);
        Task<long> CountValidReportsByUserIdAsync(string userId);
        Task<long> CountInvalidReportsByUserIdAsync(string userId);
        Task<IEnumerable<Report>> GetReportsByPostIdAsync(string postId);
    }
}
