using Feed.Core.Common.Constants;
using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.Specs;
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
            return await _reports.CountDocumentsAsync(_ => _.ReporterId == userId && _.Validity == true);
        }
        public async Task<long> CountInvalidReportsByUserIdAsync(string userId)
        {
            return await _reports.CountDocumentsAsync(_ => _.ReporterId == userId && _.Validity != true);
        }

        public async Task<IEnumerable<Report>> GetReportsByPostIdAsync(string postId)
        {
            return await _reports.Find(x => x.PostId == postId).ToListAsync();
        }

        public async Task<Report> CreateReportAsync(Report report)
        {
            await _reports.InsertOneAsync(report);
            return report;
        }

        private SortDefinition<Report> BuildSort(ReportSpecParams reportParams)
        {
            var sort = Builders<Report>.Sort;
            return reportParams.Sort?.ToLower() == SortConstants.Ascending
                ? sort.Ascending(x => x.ModifiedAt)
                : sort.Descending(x => x.ModifiedAt);
        }

        public async Task<Pagination<Report>> GetReportsAsync(ReportSpecParams reportSpecParams)
        {
            var filter = Builders<Report>.Filter.Eq(x => x.IsDeleted, false);
            var sort = BuildSort(reportSpecParams);

            var dataTask = _reports.Find(filter)
                                   .Sort(sort)
                                   .Skip((reportSpecParams.PageIndex - 1) * reportSpecParams.PageSize)
                                   .Limit(reportSpecParams.PageSize)
                                   .ToListAsync();
            var countTask = _reports.CountDocumentsAsync(filter);

            await Task.WhenAll(dataTask, countTask);

            return new Pagination<Report>(reportSpecParams.PageIndex, reportSpecParams.PageSize, countTask.Result, dataTask.Result);
        }

        public async Task<bool> UpdateReportValidity(string reportId, bool? validity)
        {
            var filter = Builders<Report>.Filter.And(
                Builders<Report>.Filter.Eq(x => x.IsDeleted, false),
                Builders<Report>.Filter.Eq(x => x.Id, reportId));
            var update = Builders<Report>.Update.Set(x => x.Validity, validity);
            var result = await _reports.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                _logger.LogWarning("Cannot update report validity to non-existent report: {reportId}", reportId);
                throw new NotFoundException(reportId);
            }

            if (result.ModifiedCount == 0)
            {
                _logger.LogError("Failed to update report validity to report {reportId}", reportId);
                throw new DatabaseException($"Failed to update report validity to report {reportId}");
            }

            return true;
        }
    }
}
