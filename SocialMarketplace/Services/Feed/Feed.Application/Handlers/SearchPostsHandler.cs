using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class SearchPostsHandler : IRequestHandler<SearchPostsCommand, IEnumerable<PostDto>>
    {
        private readonly ILogger<SearchPostsHandler> _logger;
        private readonly IRecommendationService _recommendationService;
        private readonly IPostRepository _postRepo;
        private readonly IPostMappingService _postMappingService;

        public SearchPostsHandler(ILogger<SearchPostsHandler> logger, IRecommendationService recommendationService,
            IPostRepository postRepo, IPostMappingService postMappingService)
        {
            _logger = logger;
            _recommendationService = recommendationService;
            _postRepo = postRepo;
            _postMappingService = postMappingService;
        }
        public async Task<IEnumerable<PostDto>> Handle(SearchPostsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PostSpecParams.Search == null)
                {
                    return [];
                }

                if (request.PostSpecParams.UserId != null)
                {
                    var posts = await _postRepo.SearchPostsByUserIdAsync(request.PostSpecParams.UserId, request.PostSpecParams.Search);
                    return await _postMappingService.MapToDtosAsync(posts);
                }

                var relevantPostIds = await _recommendationService.SearchPosts(request.PostSpecParams.Search);
                var relevantPosts = new List<Post>();
                foreach (var id in relevantPostIds)
                {
                    var tempRelevantPost = await _postRepo.GetPostAsync(id);
                    if (tempRelevantPost == null)
                    {
                        _logger.LogWarning("Getting post by post id {postId} not found", id);
                        continue;
                    }
                    relevantPosts.Add(tempRelevantPost);
                }
                var relevantaPostDtos = await _postMappingService.MapToDtosAsync(relevantPosts);
                return relevantaPostDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in searching posts by keyword {keyword}: {errorMsg}", request.PostSpecParams.Search, ex.Message);
                throw;
            }
        }
    }
}
