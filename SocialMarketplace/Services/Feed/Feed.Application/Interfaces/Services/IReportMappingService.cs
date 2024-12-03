using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Interfaces.Services
{
    public interface IReportMappingService
    {
        Task<ReportDto> MapToDtoAsync(Report report, CancellationToken token = default);
        Task<List<ReportDto>> MapToDtosAsync(IEnumerable<Report> posts, CancellationToken token = default);
    }
}
