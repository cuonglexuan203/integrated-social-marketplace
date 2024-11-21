
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
    public class AddReactionToCommentHandler : IRequestHandler<AddReactionToCommentCommand, ReactionDto>
    {
        private readonly ILogger<AddReactionToCommentHandler> _logger;
        private readonly ICommentRepository _comment;
        private readonly IIdentityService _identityService;

        public AddReactionToCommentHandler(ILogger<AddReactionToCommentHandler> logger, ICommentRepository comment, IIdentityService identityService)
        {
            _logger = logger;
            _comment = comment;
            _identityService = identityService;
        }
        public async Task<ReactionDto> Handle(AddReactionToCommentCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityService.GetUserDetailsAsync(request.UserId)
                ?? throw new NotFoundException($"User with id {request.UserId} does not exist");

            var reaction = new Reaction()
            {
                User = userDetails,
                Type = (ReactionType)request.ReactionType
            };

            var result = await _comment.AddReacionToCommentAsync(request.CommentId, reaction);
            return FeedMapper.Mapper.Map<ReactionDto>(result);
        }
    }
}
