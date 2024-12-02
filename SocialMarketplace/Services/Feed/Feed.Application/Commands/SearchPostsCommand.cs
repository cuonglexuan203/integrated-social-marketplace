using Feed.Application.DTOs;
using Feed.Core.Specs;
using MediatR;

namespace Feed.Application.Commands
{
    public class SearchPostsCommand: IRequest<IEnumerable<PostDto>>
    {
        public PostSpecParams PostSpecParams { get; set; }

        public SearchPostsCommand(PostSpecParams postSpecParams)
        {
            PostSpecParams = postSpecParams;
        }
    }
}
