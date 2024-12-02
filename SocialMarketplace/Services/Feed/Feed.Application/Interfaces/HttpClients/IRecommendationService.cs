namespace Feed.Application.Interfaces.HttpClients
{
    public interface IRecommendationService
    {
        Task<float> GetSentimentAnalysisScoreAsync(string commentText);
        Task<IEnumerable<string>> GetRelevantPostIdsAsync(string postId, int skip = 0, int take = 5);
        Task<IEnumerable<string>> SearchPosts(string keyword, int skip = 0, int take = 10);
    }
}
