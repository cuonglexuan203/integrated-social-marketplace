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
        private readonly IPostMappingService _postMappingService;
        private readonly IUserShareRepository _userShareRepository;

        public CreatePostHandler(IIdentityService identityService, IPostRepository postRepository, ICloudinaryService cloudinaryService, 
            ILogger<CreatePostHandler> logger, IPostMappingService postMappingService, IUserShareRepository userShareRepository)
        {
            _identityService = identityService;
            _postRepo = postRepository;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _postMappingService = postMappingService;
            _userShareRepository = userShareRepository;
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
                var postDto = await _postMappingService.MapToDtoAsync(result);
                //
                if(request.SharedPostId != null)
                {
                    try
                    {
                        var userShare = new UserShare
                        {
                            PostId = request.SharedPostId,
                            UserId = request.UserId,
                        };
                        await _userShareRepository.CreateUserShareAsync(userShare);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error in creating the user share: {msg}", ex.Message);
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
