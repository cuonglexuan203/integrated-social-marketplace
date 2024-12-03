using Feed.Application.DTOs;
using Feed.Core.Specs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetReportsQuery : IRequest<Pagination<ReportDto>>
    {
        public ReportSpecParams ReportSpecParams { get; set; }
        public GetReportsQuery(ReportSpecParams reportSpecParams)
        {
            ReportSpecParams = reportSpecParams;
        }
    }
}
