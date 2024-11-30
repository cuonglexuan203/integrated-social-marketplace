using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Feed.Infrastructure.Services.HttpClients
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ILogger<RecommendationService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecommendationService(ILogger<RecommendationService> logger, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(configuration["Microservices:Recommendation:BaseUrl"]);

            var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString()
                ?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        public async Task<float> GetSentimentAnalysisScoreAsync(string commentText)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"analyze-sentiment", new {commentText});
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<SentimentAnalysisDto>();
                return result.SentimentScore;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in getting Sentiment Analysis Score for commentText: {commentText}", commentText);
                return 0;
            }
        }
    }
}
