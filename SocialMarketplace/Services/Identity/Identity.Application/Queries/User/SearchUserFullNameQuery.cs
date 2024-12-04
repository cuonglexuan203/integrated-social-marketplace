using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Queries.User
{
    public class SearchUserFullNameQuery : IRequest<Pagination<UserDetailsResponseDTO>>
    {
        public UserSpecParams UserSpecParams { get; set; }
        public SearchUserFullNameQuery(UserSpecParams userSpecParams)
        {
            UserSpecParams = userSpecParams;
        }
    }

    public class SearchUserNameHandler : IRequestHandler<SearchUserFullNameQuery, Pagination<UserDetailsResponseDTO>>
    {
        private readonly IIdentityService _identityService;

        public SearchUserNameHandler(ILogger<SearchUserNameHandler> logger, IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Pagination<UserDetailsResponseDTO>> Handle(SearchUserFullNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SearchUserFullName(request.UserSpecParams);
            return result;
        }
    }
}
