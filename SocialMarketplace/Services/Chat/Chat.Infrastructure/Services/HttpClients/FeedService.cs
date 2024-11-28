using Chat.Application.Common.Models;
using Chat.Application.Dtos;
using Chat.Application.Interfaces.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Chat.Infrastructure.Services.HttpClients
{
    public class FeedService : IFeedService
    {
        private readonly ILogger<FeedService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public FeedService(ILogger<FeedService> logger, HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(configuration["Microservices:Feed:BaseUrl"]);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString()
                ?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = _httpContextAccessor.HttpContext?.Request.Query["access_token"].FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        public async Task<PostReferenceDto> GetPostAsync(string postId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Post/GetPost/{postId}", cancellationToken: cancellationToken);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ReturnResult<PostReferenceDto>>();
                return result.Result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in getting post {postId}: {errorMsg}", postId, ex.Message);
                return null;
            }
        }
    }
}
