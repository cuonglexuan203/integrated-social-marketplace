using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Infrastructure.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Feed.Infrastructure.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ILogger<ReportRepository> _logger;
        private readonly IMongoCollection<Report> _reports;

        public ReportRepository(ILogger<ReportRepository> logger, IFeedContext feedContext)
        {
            _logger = logger;
            _reports = feedContext.Reports;
        }

        

        public async Task<long> CountReportsByTargetUserIdAsync(string userId)
        {
            return await _reports.CountDocumentsAsync(_ => _.TargetUserId == userId);
        }

        public async Task<long> CountReportsByUserIdAsync(string userId)
        {
            return await _reports.CountDocumentsAsync(_ => _.ReporterId == userId);
        }

        public async Task<long> CountValidReportsByUserIdAsync(string userId)
        {
            return await _reports.CountDocumentsAsync(_ => _.ReporterId == userId && _.Validity);
        }
        public async Task<long> CountInvalidReportsByUserIdAsync(string userId)
        {
            return await _reports.CountDocumentsAsync(_ => _.ReporterId == userId && !_.Validity);
        }

        public async Task<IEnumerable<Report>> GetReportsByPostIdAsync(string postId)
        {
            return await _reports.Find(x => x.PostId == postId).ToListAsync();
        }
    }
}
