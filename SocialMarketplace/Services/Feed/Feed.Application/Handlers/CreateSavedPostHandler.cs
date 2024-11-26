using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Interfaces.Services;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class CreateSavedPostHandler : IRequestHandler<CreateSavedPostCommand, SavedPostDto>
    {
        private readonly ISavedPostRepository _savedPostRepo;
        private readonly ILogger<CreateSavedPostHandler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostMappingService _postMappingService;

        public CreateSavedPostHandler(ISavedPostRepository savedPostRepo, ILogger<CreateSavedPostHandler> logger,IPostRepository postRepo, 
            IHttpContextAccessor httpContextAccessor, IPostMappingService postMappingService)
        {
            _savedPostRepo = savedPostRepo;
            _logger = logger;
            _postRepo = postRepo;
            _httpContextAccessor = httpContextAccessor;
            _postMappingService = postMappingService;
        }
        public async Task<SavedPostDto> Handle(CreateSavedPostCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                _logger.LogError("userId not found in the JWT token");
                throw new NotFoundException("userId not found in the JWT token");
            }

            if(!await _postRepo.IsPostExistsAsync(request.PostId))
            {
                _logger.LogError("Post not found: post id {postId}", request.PostId);
                throw new NotFoundException("Post not found");
            }

            var savedPost = await _savedPostRepo.SavePostAsync(userId, request.PostId, cancellationToken);
            var savedPostDto = await _postMappingService.MapToDtoAsync(savedPost);
            return savedPostDto;
        }
    }
}
