namespace Feed.Application.Interfaces.Services
{
    public interface IPostScoringService
    {
        Task<float> CalculateFinalPostScore(string postId, DateTimeOffset postCreatedAt);
    }
}
