using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class CreateSavedPostHandler : IRequestHandler<CreateSavedPostCommand, SavedPostDto>
    {
        private readonly ISavedPostRepository _savedPostRepo;
        private readonly ILogger<CreateSavedPostHandler> _logger;
        private readonly IPostRepository _postRepo;

        public CreateSavedPostHandler(ISavedPostRepository savedPostRepo, ILogger<CreateSavedPostHandler> logger,IPostRepository postRepo)
        {
            _savedPostRepo = savedPostRepo;
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<SavedPostDto> Handle(CreateSavedPostCommand request, CancellationToken cancellationToken)
        {
            var savedPost = await _savedPostRepo.SavePostAsync(request.UserId, request.PostId, cancellationToken);
            var post = await _postRepo.GetPostAsync(savedPost.PostId, cancellationToken);
            var savedPostDto = FeedMapper.Mapper.Map<SavedPostDto>(post);
            savedPostDto.SavedAt = savedPost.SavedAt;
            return savedPostDto;
        }
    }
}
