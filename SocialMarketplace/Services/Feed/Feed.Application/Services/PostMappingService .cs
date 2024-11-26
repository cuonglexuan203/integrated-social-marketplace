using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Application.Mappers;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Services
{
    public class PostMappingService : IPostMappingService
    {
        private readonly IPostRepository _postRepo;
        private readonly ILogger<PostMappingService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public PostMappingService(
            IPostRepository postRepository,
            ILogger<PostMappingService> logger,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService
            )
        {
            _postRepo = postRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }
        public async Task<PostDto> MapToDtoAsync(Post post, CancellationToken token = default)
        {
            // Validate input
            ArgumentNullException.ThrowIfNull(post, nameof(post));

            // Initial mapping
            var postDto = FeedMapper.Mapper.Map<PostDto>(post);

            try
            {
                // Shared Post Mapping
                await MapSharedPostAsync(post, postDto, token);

                // User Following Status Mapping
                await MapUserFollowingStatusAsync(post, postDto, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in mapping post {postId} to postDto", post.Id);
            }

            return postDto;
        }

        private async Task MapSharedPostAsync(Post post, PostDto postDto, CancellationToken token)
        {
            if (string.IsNullOrEmpty(post.SharedPostId))
                return;

            try
            {
                var sharedPost = await _postRepo.GetPostAsync(post.SharedPostId);

                if (sharedPost != null)
                {
                    postDto.SharedPost = FeedMapper.Mapper.Map<PostDto>(sharedPost);
                }
                else
                {
                    _logger.LogWarning(
                        "Shared post {SharedPostId} not found for post {PostId}",
                        post.SharedPostId,
                        post.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving shared post {SharedPostId} for post {PostId}",
                    post.SharedPostId,
                    post.Id);
            }
        }

        private async Task MapUserFollowingStatusAsync(Post post, PostDto postDto, CancellationToken token)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User id not found in the token - Cannot map the IsFollowing property of post {postId}", post.Id);
                return;
            }

            if (post.User == null)
            {
                _logger.LogWarning("Post user is null for post {postId}", post.Id);
                return;
            }

            try
            {
                var isFollowing = await _identityService.IsUserFollowingAsync(
                    userId,
                    post.User.Id,
                    cancellationToken: token);
                if(isFollowing == null)
                {
                    _logger.LogWarning(
                    "Cannot check following status for user {UserId} and post author {PostAuthorId}",
                    userId,
                    post.User.Id);
                    return;
                }

                postDto.User.IsFollowing = isFollowing;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error checking following status for user {UserId} and post author {PostAuthorId}",
                    userId,
                    post.User.Id);
            }
        }

        // improve: parallel
        public async Task<List<PostDto>> MapToDtosAsync(IEnumerable<Post> posts, CancellationToken token = default)
        {
            var result = new List<PostDto>();
            foreach (var post in posts)
            {
                try
                {
                    result.Add(await MapToDtoAsync(post, token));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in mapping post {postId} to DTO: {errorMsg}", post.Id, ex.Message);
                }
            }
            return result;
        }
        public async Task<SavedPostDto> MapToDtoAsync(SavedPost savedPost, CancellationToken token = default)
        {
            var post = await _postRepo.GetPostAsync(savedPost.PostId, token);
            var savedPostDto = FeedMapper.Mapper.Map<SavedPostDto>(await MapToDtoAsync(post, token));
            savedPostDto.SavedAt = savedPost.SavedAt;
            return savedPostDto;
        }

        public async Task<List<SavedPostDto>> MapToDtosAsync(IEnumerable<SavedPost> savedPosts, CancellationToken token = default)
        {
            var result = new List<SavedPostDto>();
            foreach (var savedPost in savedPosts)
            {
                try
                {
                    result.Add(await MapToDtoAsync(savedPost, token));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in mapping post {postId} to DTO: {errorMsg}", savedPost.PostId, ex.Message);
                }
            }
            return result;
        }
    }
}
