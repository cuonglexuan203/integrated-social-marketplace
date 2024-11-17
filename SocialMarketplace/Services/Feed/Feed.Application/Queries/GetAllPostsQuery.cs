using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetAllPostsQuery : IRequest<IList<PostDto>>
    {
    }
}