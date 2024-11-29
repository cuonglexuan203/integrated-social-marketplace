namespace Feed.Core.Repositories
{
    public interface IUserReactionRepository
    {
        Task<long> CountTotalReactionsByUserIdAsync(string userId);
        Task<long> CountTotalReactionsByPostIdAsync(string postId);
    }
}
