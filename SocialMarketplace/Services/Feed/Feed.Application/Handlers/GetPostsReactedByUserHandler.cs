
using Feed.Application.DTOs;
using Feed.Application.Extensions;
using Feed.Application.Interfaces.Services;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetPostsReactedByUserHandler : IRequestHandler<GetPostsReactedByUserQuery, Pagination<PostDto>>
    {
        private readonly ILogger<GetPostsReactedByUserHandler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostMappingService _postMappingService;

        public GetPostsReactedByUserHandler(ILogger<GetPostsReactedByUserHandler> logger, IPostRepository postRepo, IHttpContextAccessor httpContextAccessor,
            IPostMappingService postMappingService)
        {
            _logger = logger;
            _postRepo = postRepo;
            _httpContextAccessor = httpContextAccessor;
            _postMappingService = postMappingService;
        }
        public async Task<Pagination<PostDto>> Handle(GetPostsReactedByUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                _logger.LogError("userId not found in the JWT token");
                throw new NotFoundException("userId not found in the JWT token");
            }

            var postPage = await _postRepo.GetPostsReactedByUserIdAsync(userId, request.ReactionSpecParams);
            var postDtoPage = await postPage.MapAsync(_postMappingService.MapToDtosAsync);

            return postDtoPage;
        }
    }
}
