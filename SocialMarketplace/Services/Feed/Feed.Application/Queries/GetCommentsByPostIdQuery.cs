
using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetCommentsByPostIdQuery: IRequest<IList<CommentDto>>
    {
        public string PostId { get; set; } = default!;
    }
}
