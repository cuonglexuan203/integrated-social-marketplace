
using Feed.Application.DTOs;
using Feed.Core.Specs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetPostsQuery: IRequest<Pagination<PostDto>>
    {
        public PostSpecParams PostSpecParams { get; set; }
        public GetPostsQuery(PostSpecParams postSpecParams)
        {
            PostSpecParams = postSpecParams;
        }
    }
}
