using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Commands
{
    public class FollowUserCommand : IRequest<bool>
    {
        public string FollowerId { get; set; } = default!;
        public string FollowedId { get; set; } = default!;
    }
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, bool>
    {
        private readonly IUserFollowService _userFollowService;

        public FollowUserCommandHandler(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }

        public async Task<bool> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userFollowService.FollowUserAsync(request.FollowerId, request.FollowedId, cancellationToken);
            return result;

        }
    }
}
