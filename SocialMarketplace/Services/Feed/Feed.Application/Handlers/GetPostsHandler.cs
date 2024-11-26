using Feed.Application.DTOs;
using Feed.Application.Extensions;
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

        public GetPostsHandler(ILogger<GetPostsHandler> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<Pagination<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var postPage = await _postRepo.GetPostsAsync(request.PostSpecParams, cancellationToken);

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
