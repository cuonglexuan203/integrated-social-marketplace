using Feed.Application.DTOs;
using Feed.Core.Specs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetPostsReactedByUserQuery : IRequest<Pagination<PostDto>>
    {
        public ReactionSpecParams ReactionSpecParams { get; set; }

        public GetPostsReactedByUserQuery(ReactionSpecParams reactionSpecParams)
        {
            ReactionSpecParams = reactionSpecParams;
        }
    }
}
