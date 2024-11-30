using Feed.Application.DTOs;
using Feed.Core.Enums;
using MediatR;

namespace Feed.Application.Commands
{
    public class CreateReportCommand: IRequest<ReportDto>
    {
        public string PostId { get; set; }
        public string ReporterId { get; set; }
        public string TargetUserId { get; set; }
        public string ContentText { get; set; }
        public ReportType ReportType { get; set; }
        public float ReporterCredibilityScore { get; set; }
    }
}
