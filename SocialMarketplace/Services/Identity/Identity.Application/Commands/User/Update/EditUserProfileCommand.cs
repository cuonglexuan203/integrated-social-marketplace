using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Identity.Application.Commands.User.Update
{
    public class EditUserProfileCommand : IRequest<UserDetailsResponseDTO>
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<string>? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    public class EditUserProfileCommandHandler : IRequestHandler<EditUserProfileCommand, UserDetailsResponseDTO>
    {
        private readonly ILogger<EditUserProfileCommand> _logger;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;

        public EditUserProfileCommandHandler(ILogger<EditUserProfileCommand> logger, IIdentityService identityService, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _identityService = identityService;
            _httpContext = httpContext;
        }
        public async Task<UserDetailsResponseDTO> Handle(EditUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.HttpContext.User.FindFirstValue("userId");
            if (userId == null)
            {
                _logger.LogError("UserId not found in the JWT token");
                throw new NotFoundException("UserId not found in the JWT token");
            }
            var (result, userDetailsDto) = await _identityService.UpdateUserProfileAsync(userId, request);
            if (!result.Succeeded)
            {
                _logger.LogError("Error in update user profile: userId {userId}. Error: {errors}", userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new CustomValidationException(result.Errors);
            }

            return userDetailsDto!;
        }
    }
}
