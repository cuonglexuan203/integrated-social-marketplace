﻿
using Feed.Application.Interfaces.HttpClients;
using Feed.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Feed.Infrastructure.Services.HttpClients
{
    public class IdentityService : IIdentityService,IDisposable
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
        public void Dispose() => _client?.Dispose();

        public async Task<CompactUser> GetUserDetails(string userId)
        {
            try
            {
                var response = await _client.GetAsync($"api/v1/User/GetUserDetails/{userId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CompactUser>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user details for userId: {UserId}", userId);
                throw;
            }
        }
    }
}