using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class CreateReportHandler : IRequestHandler<CreateReportCommand, ReportDto>
    {
        private readonly ILogger<CreateReportCommand> _logger;
        private readonly IReportRepository _reportRepository;

        public CreateReportHandler(ILogger<CreateReportCommand> logger, IReportRepository reportRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
        }
        public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var report = new Report
            {
                 PostId = request.PostId,
                 ReporterId = request.ReporterId,
                 TargetUserId = request.TargetUserId,
                 ContentText = request.ContentText,
                 ReportType = request.ReportType
            };

            report.CalculateReportImpactScore(request.ReporterCredibilityScore);

            var createdReport = await _reportRepository.CreateReportAsync(report);

            return FeedMapper.Mapper.Map<ReportDto>(createdReport);
        }
    }
}
