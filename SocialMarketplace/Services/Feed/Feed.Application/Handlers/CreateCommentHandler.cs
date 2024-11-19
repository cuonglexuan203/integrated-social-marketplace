﻿using Feed.Application.Commands;
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
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly ILogger<CreateCommentHandler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IIdentityService _identityService;
        private readonly ICloudinaryService _cloudinaryService;

        public CreateCommentHandler(ILogger<CreateCommentHandler> logger, IPostRepository postRepo, IIdentityService identityService, ICloudinaryService cloudinaryService)
        {
            _logger = logger;
            _postRepo = postRepo;
            _identityService = identityService;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityService.GetUserDetailsAsync(request.UserId) ?? throw new BadRequestException("User id does not exist");

            if(!await _postRepo.IsPostExistsAsync(request.PostId))
                throw new PostNotFoundException(request.PostId);

            var comment = new Comment()
            {
                PostId = request.PostId,
                CommentText = request.CommentText,
                User = userDetails
            };

            if (request.Media is not null)
            {
                var mediaResult = await _cloudinaryService.UploadSingleFileAsync(request.Media);
                comment.Media = new List<Media>() { FeedMapper.Mapper.Map<Media>(mediaResult) };
            }

            var result = await _postRepo.AddCommentToPostAsync(comment);

            return FeedMapper.Mapper.Map<CommentDto>(result);
        }
    }
}
