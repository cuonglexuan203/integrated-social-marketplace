using Feed.Application.DTOs;

namespace Feed.Application.Interfaces.HttpClients
{
    public interface IRecommendationService
    {
        Task<float> GetSentimentAnalysisScoreAsync(string commentText);
    }
}
