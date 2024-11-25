
using Feed.Application.Commands;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class UnsavePostHandler : IRequestHandler<UnsavePostCommand, bool>
    {
        private readonly ILogger<UnsavePostHandler> _logger;
        private readonly ISavedPostRepository _savedPostRepo;

        public UnsavePostHandler(ILogger<UnsavePostHandler> logger, ISavedPostRepository savedPostRepo)
        {
            _logger = logger;
            _savedPostRepo = savedPostRepo;
        }
        public async Task<bool> Handle(UnsavePostCommand request, CancellationToken cancellationToken)
        {
            var result = await _savedPostRepo.RemoveSavedPostAsync(request.UserId, request.PostId, cancellationToken);
            return result;
        }
    }
}
