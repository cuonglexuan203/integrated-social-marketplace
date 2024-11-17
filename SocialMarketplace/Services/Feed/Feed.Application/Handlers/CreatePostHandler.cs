using AutoMapper;
using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Core.Entities;
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
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePostHandler> _logger;

        public CreatePostHandler(IIdentityService identityService, IPostRepository postRepository, ICloudinaryService cloudinaryService, IMapper mapper, ILogger<CreatePostHandler> logger)
        {
            _identityService = identityService;
            _postRepository = postRepository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userDetails = await _identityService.GetUserDetails(request.UserId);
                var mediaResult = await _cloudinaryService.UploadMultipleFilesAsync(request.Files, "SocialMarketplace");
                var post = new Post()
                {
                    User = userDetails,
                    Media = _mapper.Map<List<Media>>(mediaResult),
                    ContentText = request.ContentText,
                };
                var result = await _postRepository.CreatePost(post);
                return _mapper.Map<PostDto>(result);
            }
            catch (Exception ex) {
                _logger.LogError("An error occur while creating the Post: {ErrorMessage}", ex.Message);
                return null; ;
            }
        }
    }
}
