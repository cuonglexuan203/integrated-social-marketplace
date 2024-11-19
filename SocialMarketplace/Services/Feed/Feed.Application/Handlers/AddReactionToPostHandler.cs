using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Mappers;
using Feed.Core.Enums;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using Feed.Core.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class AddReactionToPostHandler : IRequestHandler<AddReactionToPostCommand, ReactionDto>
    {
        private readonly ILogger<AddReactionToPostCommand> _logger;
        private readonly IPostRepository _postRepo;
        private readonly IIdentityService _identityService;

        public AddReactionToPostHandler(ILogger<AddReactionToPostCommand> logger, IPostRepository postRepo, IIdentityService identityService)
        {
            _logger = logger;
            _postRepo = postRepo;
            _identityService = identityService;
        }
        public async Task<ReactionDto> Handle(AddReactionToPostCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityService.GetUserDetailsAsync(request.UserId) 
                ?? throw new BadRequestException($"User with id {request.UserId} does not exist");

            if (!await _postRepo.IsPostExistsAsync(request.PostId))
            {
                _logger.LogError("Cannot add reaction to non-existent post: {postId}", request.PostId);
                throw new PostNotFoundException(request.PostId);
            }

            var reaction = new Reaction()
            {
                User = userDetails,
                Type = (ReactionType)request.ReactionType
            };

            var result = await _postRepo.AddReacionToPostAsync(request.PostId, reaction);
            return FeedMapper.Mapper.Map<ReactionDto>(result);
        }
    }
}
