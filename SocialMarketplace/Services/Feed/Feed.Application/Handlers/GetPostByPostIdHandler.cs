
using Feed.Application.DTOs;
using Feed.Application.Interfaces.Services;
using Feed.Application.Queries;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetPostByPostIdHandler : IRequestHandler<GetPostByPostIdQuery, PostDto>
    {
        private readonly ILogger<GetPostByPostIdQuery> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IPostMappingService _postMappingService;

        public GetPostByPostIdHandler(ILogger<GetPostByPostIdQuery> logger, IPostRepository postRepo, IPostMappingService postMappingService)
        {
            _logger = logger;
            _postRepo = postRepo;
            _postMappingService = postMappingService;
        }
        public async Task<PostDto> Handle(GetPostByPostIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepo.GetPostAsync(request.PostId, cancellationToken);
            if(post == null)
            {
                _logger.LogError("Post not found: post id {postId}", request.PostId);
                throw new NotFoundException("Post not found");
            }

            var postDto = await _postMappingService.MapToDtoAsync(post);
            return postDto;
        }
    }
}
