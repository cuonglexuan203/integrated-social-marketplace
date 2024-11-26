
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Commands
{
    public class UnfollowUserCommand : IRequest<bool>
    {
        public string FollowerId { get; set; } = default!;
        public string FollowedId { get; set; } = default!;
    }
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, bool>
    {
        private readonly IUserFollowService _userFollowService;

        public UnfollowUserCommandHandler(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }

        public async Task<bool> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userFollowService.UnfollowUserAsync(request.FollowerId, request.FollowedId, cancellationToken);
            return result;

        }
    }
}
