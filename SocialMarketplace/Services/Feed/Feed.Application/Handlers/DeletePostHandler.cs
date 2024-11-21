
using Feed.Application.Commands;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly ILogger<DelegatingHandler> _logger;
        private readonly IPostRepository _postRepo;

        public DeletePostHandler(ILogger<DelegatingHandler> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var result = await _postRepo.SoftDeleteAsync(request.PostId, cancellationToken);
            return result;
        }
    }
}
