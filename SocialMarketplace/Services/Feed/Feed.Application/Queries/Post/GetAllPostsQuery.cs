using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries.Post
{
    public class GetAllPostsQuery : IRequest<IList<PostResponse>>
    {
    }
}