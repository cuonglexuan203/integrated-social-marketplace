using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetAllReacionsByPostIdQuery: IRequest<IList<ReactionDto>>
    {
        public string PostId { get; set; }
    }
}
