
using Feed.Application.Common.Models;
using Feed.Application.Interfaces.HttpClients;
using Feed.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Feed.Infrastructure.Services.HttpClients
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _client;
        private readonly ILogger<IdentityService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(HttpClient client, ILogger<IdentityService> logger ,IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            _client.BaseAddress = new Uri(configuration["Services:Identity:BaseUrl"]);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString()
                ?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(accessToken))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<CompactUser> GetUserDetailsAsync(string userId)
        {
            try
            {
                var response = await _client.GetAsync($"User/GetUserDetails/{userId}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ReturnResult<CompactUser>>();
                return result.Result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error getting user details for userId: {UserId}", userId);
                return null;
            }
        }
    }
}
