
using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetAllUserPostsQuery: IRequest<IList<PostDto>>
    {
        public string UserId { get; set; }

        public GetAllUserPostsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
