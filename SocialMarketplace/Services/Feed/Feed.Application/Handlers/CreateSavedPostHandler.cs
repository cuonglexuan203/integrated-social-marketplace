﻿using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Feed.Application.Handlers
{
    public class CreateSavedPostHandler : IRequestHandler<CreateSavedPostCommand, SavedPostDto>
    {
        private readonly ISavedPostRepository _savedPostRepo;
        private readonly ILogger<CreateSavedPostHandler> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateSavedPostHandler(ISavedPostRepository savedPostRepo, ILogger<CreateSavedPostHandler> logger,IPostRepository postRepo, IHttpContextAccessor httpContextAccessor)
        {
            _savedPostRepo = savedPostRepo;
            _logger = logger;
            _postRepo = postRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SavedPostDto> Handle(CreateSavedPostCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                _logger.LogError("userId not found in the JWT token");
                throw new NotFoundException("userId not found in the JWT token");
            }

            if(!await _postRepo.IsPostExistsAsync(request.PostId))
            {
                _logger.LogError("Post not found: post id {postId}", request.PostId);
                throw new NotFoundException("Post not found");
            }

            var savedPost = await _savedPostRepo.SavePostAsync(userId, request.PostId, cancellationToken);
            var post = await _postRepo.GetPostAsync(savedPost.PostId, cancellationToken);
            var savedPostDto = FeedMapper.Mapper.Map<SavedPostDto>(post);
            if (!string.IsNullOrEmpty(post.SharedPostId))
            {
                try
                {
                    var sharedPost = await _postRepo.GetPostAsync(post.SharedPostId, cancellationToken);
                    savedPostDto.SharedPost = FeedMapper.Mapper.Map<PostDto>(sharedPost);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in getting the shared post {sharedPostId} - post id {postId}: {errorMessage}", post.SharedPostId, post.Id, ex.Message);
                    //throw;
                }
            }
            savedPostDto.SavedAt = savedPost.SavedAt;
            return savedPostDto;
        }
    }
}
