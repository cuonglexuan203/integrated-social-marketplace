
using Feed.Application.Commands;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class RemoveReactionFromPostHandler : IRequestHandler<RemoveReactionFromPostCommand, bool>
    {
        private readonly ILogger<RemoveReactionFromPostHandler> _logger;
        private readonly IPostRepository _postRepo;

        public RemoveReactionFromPostHandler(ILogger<RemoveReactionFromPostHandler> logger,IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<bool> Handle(RemoveReactionFromPostCommand request, CancellationToken cancellationToken)
        {
            var result = await _postRepo.RemoveReactionFromPostAsync(request.PostId, request.UserId, cancellationToken);
            return result;
        }
    }
}
