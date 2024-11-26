
using Feed.Application.DTOs;
using Feed.Core.Specs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetPostsCommentedByUserIdQuery: IRequest<Pagination<PostDto>>
    {
        CommentSpecParams CommentParams { get; set; }

        public GetPostsCommentedByUserIdQuery(CommentSpecParams commentParams)
        {
            CommentParams = commentParams;
        }
    }
}
