
using Feed.Application.Commands;
using Feed.Core.Exceptions;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class UnsavePostHandler : IRequestHandler<UnsavePostCommand, bool>
    {
        private readonly ILogger<UnsavePostHandler> _logger;
        private readonly ISavedPostRepository _savedPostRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnsavePostHandler(ILogger<UnsavePostHandler> logger, ISavedPostRepository savedPostRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _savedPostRepo = savedPostRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(UnsavePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                _logger.LogError("userId not found in the JWT token");
                throw new NotFoundException("userId not found in the JWT token");
            }
            var result = await _savedPostRepo.RemoveSavedPostAsync(userId, request.PostId, cancellationToken);
            return result;
        }
    }
}
