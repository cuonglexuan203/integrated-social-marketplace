using Feed.Application.DTOs;
using Feed.Application.Extensions;
using Feed.Application.Interfaces.Services;
using Feed.Application.Queries;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetReportsHandler : IRequestHandler<GetReportsQuery, Pagination<ReportDto>>
    {
        private readonly ILogger<GetPostByPostIdHandler> _logger;
        private readonly IReportRepository _reportRepo;
        private readonly IReportMappingService _reportMappingService;

        public GetReportsHandler(ILogger<GetPostByPostIdHandler> logger, IReportRepository reportRepo,
            IReportMappingService reportMappingService)
        {
            _logger = logger;
            _reportRepo = reportRepo;
            _reportMappingService = reportMappingService;
        }
        public async Task<Pagination<ReportDto>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            var reportPage = await _reportRepo.GetReportsAsync(request.ReportSpecParams);
            var reportDtoPage = await reportPage.MapAsync(_reportMappingService.MapToDtosAsync);
            return reportDtoPage;
        }
    }
}
