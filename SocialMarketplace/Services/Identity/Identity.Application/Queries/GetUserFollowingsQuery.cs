using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Queries
{
    public class GetUserFollowingsQuery : IRequest<List<UserDetailsResponseDTO>>
    {
        public string UserId { get; set; } = default!;
    }

    public class GetUserFollowingsHandler : IRequestHandler<GetUserFollowingsQuery, List<UserDetailsResponseDTO>>
    {
        private readonly IUserFollowService _userFollowService;

        public GetUserFollowingsHandler(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }
        public async Task<List<UserDetailsResponseDTO>> Handle(GetUserFollowingsQuery request, CancellationToken cancellationToken)
        {
            var result = await _userFollowService.GetFollowingAsync(request.UserId);
            return result;
        }
    }
}
