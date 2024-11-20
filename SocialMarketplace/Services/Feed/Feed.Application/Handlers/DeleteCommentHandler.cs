using Feed.Application.Commands;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly ILogger<DeleteCommentHandler> _logger;
        private readonly ICommentRepository _commentRepo;

        public DeleteCommentHandler(ILogger<DeleteCommentHandler> logger, ICommentRepository commentRepo)
        {
            _logger = logger;
            _commentRepo = commentRepo;
        }
        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var result = await _commentRepo.SoftDeleteAsync(request.CommentId, cancellationToken);
            return result;
        }
    }
}
