using Feed.Application.Interfaces.Services;
using Feed.Core.Common.Constants;
using Feed.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Services
{
    public class UserCredibilityUpdateService: IUserCredibilityUpdateService
    {
        private readonly ILogger<UserCredibilityUpdateService> _logger;
        private readonly IUserReactionRepository _userReactionRepository;
        private readonly IUserShareRepository _userShareRepository;
        private readonly IUserCommentsRepository _userCommentsRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IPostRepository _postRepository;

        public UserCredibilityUpdateService(ILogger<UserCredibilityUpdateService> logger, IUserReactionRepository userReactionRepository,
            IUserShareRepository userShareRepository, IUserCommentsRepository userCommentsRepository, IReportRepository reportRepository,
            IPostRepository postRepository)
        {
            _logger = logger;
            _userReactionRepository = userReactionRepository;
            _userShareRepository = userShareRepository;
            _userCommentsRepository = userCommentsRepository;
            _reportRepository = reportRepository;
            _postRepository = postRepository;
        }

        public float GetBaseCredibility(string userId)
        {
            return 50;
        }

        private async Task<float> CalculatePositiveInteractionsScoreAsync(string userId)
        {
            var totalReactionsTask = _userReactionRepository.CountTotalReactionsByUserIdAsync(userId);
            var totalCommentsTask = _userCommentsRepository.CountTotalCommentsByUserIdAsync(userId);
            var totalSharesTask = _userShareRepository.CountTotalSharesByUserIdAsync(userId);
            await Task.WhenAll(totalReactionsTask, totalCommentsTask, totalSharesTask);

            return totalReactionsTask.Result +
                   WeightConstants.User.TotalComments * totalCommentsTask.Result +
                   WeightConstants.User.TotalShares * totalSharesTask.Result;
        }

        private async Task<float> CalculateNegativeInteractionsScoreAsync(string userId)
        {
            return await _reportRepository.CountReportsByTargetUserIdAsync(userId) * WeightConstants.User.NegativeInteractions;
        }

        private async Task<float> CalculateValidReportsScoreAsync(string userId)
        {
            return await _reportRepository.CountValidReportsByUserIdAsync(userId) * WeightConstants.User.ValidReports;
        }

        private async Task<float> CalculateInvalidReportsScoreAsync(string userId)
        {
            return await _reportRepository.CountInvalidReportsByUserIdAsync(userId) * WeightConstants.User.InvalidReports;
        }

        private async Task<float> CalculateHighEngagementScoreAsync(string userId)
        {
            var totalReactionsTask = _userReactionRepository.CountTotalReactionsByUserIdAsync(userId);
            var totalCommentsTask = _userCommentsRepository.CountTotalCommentsByUserIdAsync(userId);
            var totalSharesTask = _userShareRepository.CountTotalSharesByUserIdAsync(userId);

            var countTask = _postRepository.CountPostsByUserIdAsync(userId);

            await Task.WhenAll(totalReactionsTask, totalCommentsTask, totalSharesTask, countTask);

            return WeightConstants.User.HighEngagement *
                (totalReactionsTask.Result + totalCommentsTask.Result + totalSharesTask.Result) / countTask.Result;
        }

        public async Task<float> CalculateUserCredibilityScore(string userId)
        {
            return GetBaseCredibility(userId) + await CalculatePositiveInteractionsScoreAsync(userId) -
                await CalculateNegativeInteractionsScoreAsync(userId) + await CalculateValidReportsScoreAsync(userId) -
                await CalculateInvalidReportsScoreAsync(userId) + await CalculateHighEngagementScoreAsync(userId);
        }
    }
}
