
using Feed.Application.Commands;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class RemoveReactionFromCommentHandler : IRequestHandler<RemoveReactionFromCommentCommand, bool>
    {
        private readonly ILogger<RemoveReactionFromCommentHandler> _logger;
        private readonly ICommentRepository _commentRepo;

        public RemoveReactionFromCommentHandler(ILogger<RemoveReactionFromCommentHandler> logger, ICommentRepository commentRepo)
        {
            _logger = logger;
            _commentRepo = commentRepo;
        }
        public async Task<bool> Handle(RemoveReactionFromCommentCommand request, CancellationToken cancellationToken)
        {
            var result = await _commentRepo.RemoveReactionFromCommentAsync(request.CommentId, request.UserId, cancellationToken);
            return result;
        }
    }
}
