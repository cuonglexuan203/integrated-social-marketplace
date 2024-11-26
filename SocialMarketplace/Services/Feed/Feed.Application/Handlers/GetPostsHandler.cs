using Feed.Application.DTOs;
using Feed.Application.Extensions;
using Feed.Application.Interfaces.Services;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using Feed.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetPostsHandler : IRequestHandler<GetPostsQuery, Pagination<PostDto>>
    {
        private readonly ILogger<GetPostsHandler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IPostMappingService _postMappingService;

        public GetPostsHandler(ILogger<GetPostsHandler> logger, IPostRepository postRepo, IPostMappingService postMappingService)
        {
            _logger = logger;
            _postRepo = postRepo;
            _postMappingService = postMappingService;
        }
        public async Task<Pagination<PostDto>> Handle(GetPostsQuery request, CancellationToken token)
        {
            var postPage = await _postRepo.GetPostsAsync(request.PostSpecParams, token);

            var postDtoPage = await postPage.MapAsync(_postMappingService.MapToDtosAsync);
            return postDtoPage;
        }
    }
}
