
using Feed.Application.DTOs;
using Feed.Application.Mappers;
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

        public GetPostByPostIdHandler(ILogger<GetPostByPostIdQuery> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<PostDto> Handle(GetPostByPostIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _postRepo.GetPostAsync(request.PostId, cancellationToken);
            if(result == null)
            {
                _logger.LogError("Post not found: post id {postId}", request.PostId);
                throw new NotFoundException("Post not found");
            }

            var postDto = FeedMapper.Mapper.Map<PostDto>(result);
            if(!string.IsNullOrEmpty(result.SharedPostId))
            {
                try
                {
                    var sharedPost = await _postRepo.GetPostAsync(result.SharedPostId, cancellationToken);
                    postDto.SharedPost = FeedMapper.Mapper.Map<PostDto>(sharedPost);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in getting the shared post {sharedPostId} - post id {postId}: {errorMessage}", result.SharedPostId, result.Id, ex.Message);
                    //throw;
                }
            }
            return postDto;
        }
    }
}
