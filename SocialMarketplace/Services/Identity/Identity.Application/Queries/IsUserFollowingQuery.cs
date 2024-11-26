
using Identity.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Queries
{
    public class IsUserFollowingQuery: IRequest<bool>
    {
        public string FollowerId { get; set; }
        public string FollowedId { get; set; }
    }
    public class IsUserFollowingHandler : IRequestHandler<IsUserFollowingQuery, bool>
    {
        private readonly ILogger<IsUserFollowingHandler> _logger;
        private readonly IUserFollowService _userFollowService;

        public IsUserFollowingHandler(ILogger<IsUserFollowingHandler> logger, IUserFollowService userFollowService)
        {
            _logger = logger;
            _userFollowService = userFollowService;
        }
        public async Task<bool> Handle(IsUserFollowingQuery request, CancellationToken cancellationToken)
        {
            var result = await _userFollowService.IsFollowingAsync(request.FollowerId, request.FollowedId, cancellationToken);
            return result;
        }
    }
}
