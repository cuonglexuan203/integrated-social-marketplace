using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Application.Queries;
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

        public GetPostsHandler(ILogger<GetPostsHandler> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<Pagination<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var postPage = await _postRepo.GetPostsAsync(request.PostSpecParams, cancellationToken);
            var postDtos = FeedMapper.Mapper.Map<IReadOnlyList<PostDto>>(postPage.Data);
            return new Pagination<PostDto>(postPage.PageIndex, postPage.PageSize, postPage.Count, postDtos);
        }
    }
}
