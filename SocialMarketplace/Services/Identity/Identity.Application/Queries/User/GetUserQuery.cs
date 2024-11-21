using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Queries.User
{
    public class GetUserQuery : IRequest<IList<UserResponseDTO>>
    {
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IList<UserResponseDTO>>
    {
        private readonly IIdentityService _identityService;

        public GetUserQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IList<UserResponseDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _identityService.GetAllUsersAsync();
        }
    }
}
