using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Identity.Application.Commands.Auth
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }

    public class ChangePasswordHanlder : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly ILogger<ChangePasswordHanlder> _logger;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;

        public ChangePasswordHanlder(ILogger<ChangePasswordHanlder> logger, IIdentityService identityService, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _identityService = identityService;
            _httpContext = httpContext;
        }
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userName = _httpContext.HttpContext.User.FindFirstValue("name");
            if (userName == null)
            {
                _logger.LogError("UserName not found in the JWT token");
                throw new NotFoundException("UserName not found in the JWT token");
            }
            var isCurrentPasswordValid = await _identityService.SigninUserAsync(userName, request.CurrentPassword);
            if (!isCurrentPasswordValid)
            {
                _logger.LogWarning("Current password validation failed for user: {userName}.", userName);
                throw new BadRequestException("Current password is incorrect");
            }
            var passwordChangedResult = await _identityService.ChangePasswordAsync(
                userName,
                request.CurrentPassword,
                request.NewPassword
            );

            if (!passwordChangedResult.Succeeded)
            {
                _logger.LogError("Password change failed for user: {userName}. Errors: {errors}", userName, string.Join(", ", passwordChangedResult.Errors.Select(e => e.Description)));
                throw new CustomValidationException(passwordChangedResult.Errors);
            }

            _logger.LogInformation($"Password changed for user: {userName}");

            return true;
        }
    }
}
