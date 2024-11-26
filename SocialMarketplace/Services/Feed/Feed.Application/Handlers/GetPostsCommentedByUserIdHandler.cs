
using Feed.Application.DTOs;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetPostsCommentedByUserIdHandler : IRequestHandler<GetPostsCommentedByUserIdQuery, Pagination<PostDto>>
    {
        private readonly ILogger<GetPostsCommentedByUserIdHandler> _logger;
        private readonly IPostRepository _postRepo;

        public GetPostsCommentedByUserIdHandler(ILogger<GetPostsCommentedByUserIdHandler> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public Task<Pagination<PostDto>> Handle(GetPostsCommentedByUserIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
