using Feed.Application.Responses;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetAllPostsQuery: IRequest<IList<PostResponse>>
    {
    }
}
