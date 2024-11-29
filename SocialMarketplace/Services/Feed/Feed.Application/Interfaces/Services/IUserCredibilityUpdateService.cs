
namespace Feed.Application.Interfaces.Services
{
    public interface IUserCredibilityUpdateService
    {
        Task<float> CalculateUserCredibilityScore(string userId);
    }
}
