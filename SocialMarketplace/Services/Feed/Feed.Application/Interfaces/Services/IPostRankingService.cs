using Feed.Application.DTOs;
namespace Feed.Application.Interfaces.Services
{
    public interface IPostRankingService
    {
        Task<IEnumerable<string>> GetTopPostIdsByUserInteractionAsync(string userId, int take);
        Task<IEnumerable<PostDto>> GetRecommendedPostsAsync(string userId, int skip, int take);
    }
}
