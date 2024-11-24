using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetPostByPostIdQuery: IRequest<PostDto>
    {
        public string PostId { get; set; }

        public GetPostByPostIdQuery(string postId)
        {
            PostId = postId;
        }
    }
}
