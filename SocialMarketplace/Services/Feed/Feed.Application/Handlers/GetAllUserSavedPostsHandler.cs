
using Feed.Application.DTOs;
using Feed.Application.Interfaces.Services;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetAllUserSavedPostsHandler : IRequestHandler<GetAllUserSavedPostsQuery, IList<SavedPostDto>>
    {
        private readonly ILogger<GetAllUserSavedPostsHandler> _logger;
        private readonly ISavedPostRepository _savedPostRepo;
        private readonly IPostRepository _postRepo;
        private readonly IPostMappingService _postMappingService;

        public GetAllUserSavedPostsHandler(ILogger<GetAllUserSavedPostsHandler> logger, ISavedPostRepository savedPostRepo, IPostRepository postRepo,
            IPostMappingService postMappingService)
        {
            _logger = logger;
            _savedPostRepo = savedPostRepo;
            _postRepo = postRepo;
            _postMappingService = postMappingService;
        }
        public async Task<IList<SavedPostDto>> Handle(GetAllUserSavedPostsQuery request, CancellationToken token)
        {
            var savedPosts = await _savedPostRepo.GetSavedPostsAsync(request.UserId, token);

            var result = await _postMappingService.MapToDtosAsync(savedPosts, token);

            return result;
        }
    }
}
