
using Feed.Application.DTOs;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetAllUserPostsHandler : IRequestHandler<GetAllUserPostsQuery, IList<PostDto>>
    {
        private readonly ILogger<GetAllUserPostsQuery> _logger;
        private readonly IPostRepository _postRepo;

        public GetAllUserPostsHandler(ILogger<GetAllUserPostsQuery> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public Task<IList<PostDto>> Handle(GetAllUserPostsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
