
using Feed.Application.DTOs;
using Feed.Application.Extensions;
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

        public GetPostsReactedByUserHandler(ILogger<GetPostsReactedByUserHandler> logger, IPostRepository postRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _postRepo = postRepo;
            _httpContextAccessor = httpContextAccessor;
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
            var postDtoPage = postPage.Map<Post, PostDto>();

            foreach (var postDto in postDtoPage.Data)
            {
                try
                {
                    if (!string.IsNullOrEmpty(postDto.SharedPostId))
                    {
                        var sharedPost = await _postRepo.GetPostAsync(postDto.SharedPostId, cancellationToken);
                        postDto.SharedPost = FeedMapper.Mapper.Map<Post, PostDto>(sharedPost);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in getting the shared post {sharedPostId} - post id {postId}: {errorMessage}", postDto.SharedPostId, postDto.Id, ex.Message);
                    //throw;
                }
            }

            return postDtoPage;
        }
    }
}
