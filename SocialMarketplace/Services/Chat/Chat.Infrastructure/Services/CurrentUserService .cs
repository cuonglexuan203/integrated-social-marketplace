using Chat.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Chat.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CurrentUserService> _logger;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrentUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string UserId
        {
            get
            {
                try
                {
                    // Try to get sub claim first, then fall back to nameidentifier
                    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ??
                        _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogWarning("Unable to get user ID from JWT claims");
                        return null;
                    }

                    return userId;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving user ID from JWT claims");
                    return null;
                }
            }
        }

        public string Username
        {
            get
            {
                try
                {
                    return _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                        _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving username from JWT claims");
                    return null;
                }
            }
        }

        //public string Email
        //{
        //    get
        //    {
        //        try
        //        {
        //            return _httpContextAccessor.HttpContext?.User?
        //                .FindFirstValue(ClaimTypes.Email);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error retrieving email from JWT claims");
        //            return null;
        //        }
        //    }
        //}

        public IEnumerable<string> Roles
        {
            get
            {
                try
                {
                    return _httpContextAccessor.HttpContext?.User?
                        .Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                        .Select(c => c.Value)
                        .ToList() ?? new List<string>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving roles from JWT claims");
                    return new List<string>();
                }
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                try
                {
                    return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking JWT authentication status");
                    return false;
                }
            }
        }

        public string JwtToken
        {
            get
            {
                try
                {
                    var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
                    return authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring(7) : null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving JWT token from request");
                    return null;
                }
            }
        }
    }
}
