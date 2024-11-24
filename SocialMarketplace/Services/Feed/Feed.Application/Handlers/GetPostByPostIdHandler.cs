
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetPostByPostIdHandler : IRequestHandler<GetPostByPostIdQuery, PostDto>
    {
        private readonly ILogger<GetPostByPostIdQuery> _logger;
        private readonly IPostRepository _postRepo;

        public GetPostByPostIdHandler(ILogger<GetPostByPostIdQuery> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<PostDto> Handle(GetPostByPostIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _postRepo.GetPostAsync(request.PostId, cancellationToken);
            return FeedMapper.Mapper.Map<PostDto>(result);
        }
    }
}
