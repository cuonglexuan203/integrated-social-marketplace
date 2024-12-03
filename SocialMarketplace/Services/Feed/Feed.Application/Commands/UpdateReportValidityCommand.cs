using MediatR;

namespace Feed.Application.Commands
{
    public class UpdateReportValidityCommand: IRequest<bool>
    {
        public string ReportId { get; set; }
        public bool? Validity { get; set; }
    }
}
