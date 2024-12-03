
using Feed.Core.Entities;
using Feed.Core.Specs;

namespace Feed.Core.Repositories
{
    public interface IReportRepository
    {
        Task<long> CountReportsByUserIdAsync(string userId);
        Task<long> CountReportsByTargetUserIdAsync(string userId);
        Task<long> CountValidReportsByUserIdAsync(string userId);
        Task<long> CountInvalidReportsByUserIdAsync(string userId);
        Task<IEnumerable<Report>> GetReportsByPostIdAsync(string postId);
        Task<Report> CreateReportAsync(Report report);
        Task<Pagination<Report>> GetReportsAsync(ReportSpecParams reportSpecParams);
        Task<bool> UpdateReportValidity(string reportId, bool? validity);
    }
}
