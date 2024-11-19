using AutoMapper;
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
        private readonly IPostRepository _postRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<CreatePostHandler> _logger;

        public CreatePostHandler(IIdentityService identityService, IPostRepository postRepository, ICloudinaryService cloudinaryService, ILogger<CreatePostHandler> logger)
        {
            _identityService = identityService;
            _postRepository = postRepository;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ContentText) && (request.Files == null || request.Files.Length == 0))
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
                };

                if(request.Tags is not null)
                {
                    post.Tags = request.Tags;
                }

                if (request.Files is not null) {
                    var mediaResult = await _cloudinaryService.UploadMultipleFilesAsync(request.Files);
                    post.Media = FeedMapper.Mapper.Map<List<Media>>(mediaResult);
                }
                
                var result = await _postRepository.CreatePost(post);
                return FeedMapper.Mapper.Map<PostDto>(result);
            }
            catch (Exception ex) {
                _logger.LogError("An error occur while creating the Post: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}
