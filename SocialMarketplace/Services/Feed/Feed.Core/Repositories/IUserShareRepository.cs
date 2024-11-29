namespace Feed.Core.Repositories
{
    public interface IUserShareRepository
    {
        Task<long> CountTotalSharesByUserIdAsync(string userId);
        Task<long> CountTotalSharesByPostIdAsync(string postId);
    }
}
