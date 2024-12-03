using Feed.Application.DTOs;
using Feed.Application.Extensions;
using Feed.Application.Interfaces.Services;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetPostsHandler : IRequestHandler<GetPostsQuery, Pagination<PostDto>>
    {
        private readonly ILogger<GetPostsHandler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IPostMappingService _postMappingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRankingService _postRankingService;

        public GetPostsHandler(ILogger<GetPostsHandler> logger, IPostRepository postRepo, IPostMappingService postMappingService,
            IHttpContextAccessor httpContextAccessor, IPostRankingService postRankingService)
        {
            _logger = logger;
            _postRepo = postRepo;
            _postMappingService = postMappingService;
            _httpContextAccessor = httpContextAccessor;
            _postRankingService = postRankingService;
        }
        public async Task<Pagination<PostDto>> Handle(GetPostsQuery request, CancellationToken token)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;

            if (userId == null || request.PostSpecParams.UserId != null)
            {
                var postPage = await _postRepo.GetPostsAsync(request.PostSpecParams, token);

                var postDtoPage = await postPage.MapAsync(_postMappingService.MapToDtosAsync);
                return postDtoPage;
            }

            var recommendedPostDtos = await _postRankingService.GetRecommendedPostsAsync(userId,
                (request.PostSpecParams.PageIndex - 1) * request.PostSpecParams.PageSize, request.PostSpecParams.PageSize);
            //var totalCount = await _postRepo.CountPostAsync();
            return new Pagination<PostDto>
            {
                PageSize = request.PostSpecParams.PageSize,
                PageIndex = request.PostSpecParams.PageIndex,
                Data = recommendedPostDtos.ToList(),
                //Count = totalCount,
            };
        }
    }
}
