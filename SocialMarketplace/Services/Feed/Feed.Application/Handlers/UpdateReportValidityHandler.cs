using Feed.Application.Commands;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class UpdateReportValidityHandler : IRequestHandler<UpdateReportValidityCommand, bool>
    {
        private readonly ILogger<UpdateReportValidityHandler> _logger;
        private readonly IReportRepository _reportRepo;

        public UpdateReportValidityHandler(ILogger<UpdateReportValidityHandler> logger, IReportRepository reportRepo)
        {
            _logger = logger;
            _reportRepo = reportRepo;
        }
        public async Task<bool> Handle(UpdateReportValidityCommand request, CancellationToken cancellationToken)
        {
            var result = await _reportRepo.UpdateReportValidity(request.ReportId, request.Validity);
            return result;
        }
    }
}
