
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetAllUserSavedPostsHandler : IRequestHandler<GetAllUserSavedPostsQuery, IList<SavedPostDto>>
    {
        private readonly ILogger<GetAllUserSavedPostsHandler> _logger;
        private readonly ISavedPostRepository _savedPostRepo;
        private readonly IPostRepository _postRepo;

        public GetAllUserSavedPostsHandler(ILogger<GetAllUserSavedPostsHandler> logger, ISavedPostRepository savedPostRepo, IPostRepository postRepo)
        {
            _logger = logger;
            _savedPostRepo = savedPostRepo;
            _postRepo = postRepo;
        }
        public async Task<IList<SavedPostDto>> Handle(GetAllUserSavedPostsQuery request, CancellationToken cancellationToken)
        {
            var savedPosts = await _savedPostRepo.GetSavedPostsAsync(request.UserId, cancellationToken);

            var result = new List<SavedPostDto>();
            foreach (var savedPost in savedPosts)
            {
                try
                {
                    var post = await _postRepo.GetPostAsync(savedPost.PostId, cancellationToken);
                    var savedPostDto = FeedMapper.Mapper.Map<SavedPostDto>(post);
                    savedPostDto.SavedAt = savedPost.SavedAt;
                    result.Add(savedPostDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occured while getting the saved posts: {errorMessage}", ex.Message);
                }
            }

            return result;
        }
    }
}
