
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Queries
{
    public class GetUserFollowersQuery: IRequest<List<UserDetailsResponseDTO>>
    {
        public string UserId { get; set; }
        public GetUserFollowersQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserFollowersHandler : IRequestHandler<GetUserFollowersQuery, List<UserDetailsResponseDTO>>
    {
        private readonly IUserFollowService _userFollowService;

        public GetUserFollowersHandler(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }
        public async Task<List<UserDetailsResponseDTO>> Handle(GetUserFollowersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userFollowService.GetFollowersAsync(request.UserId);
            return result;
        }
    }
}
