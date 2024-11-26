using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Application.Mappers;
using Feed.Core.Entities;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IPostRepository _postRepo;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<CreatePostHandler> _logger;

        public CreatePostHandler(IIdentityService identityService, IPostRepository postRepository, ICloudinaryService cloudinaryService, ILogger<CreatePostHandler> logger)
        {
            _identityService = identityService;
            _postRepo = postRepository;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ContentText) && (request.Files == null || request.Files.Length == 0) && request.SharedPostId == null)
            {
                throw new BadRequestException("The post must have content or media file(s).");
            }

            try
            {
                var userDetails = await _identityService.GetUserDetailsAsync(request.UserId);

                var post = new Post()
                {
                    User = userDetails,
                    ContentText = request.ContentText,
                    SharedPostId = request.SharedPostId,
                };

                if(request.Tags != null)
                {
                    post.Tags = request.Tags;
                }

                if (request.Files != null) {
                    var mediaResult = await _cloudinaryService.UploadMultipleFilesAsync(request.Files);
                    post.Media = FeedMapper.Mapper.Map<List<Media>>(mediaResult);
                }
                
                var result = await _postRepo.CreatePostAsync(post);
                var postDto = FeedMapper.Mapper.Map<PostDto>(result);
                if (!string.IsNullOrEmpty(result.SharedPostId))
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
            catch (Exception ex) {
                _logger.LogError("An error occur while creating the Post: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}
