using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Core.Common.Constants;
using Feed.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Services
{
    public class PostScoringService: IPostScoringService
    {
        private readonly ILogger<PostScoringService> _logger;
        private readonly IUserReactionRepository _userReactionRepository;
        private readonly IUserCommentsRepository _userCommentsRepository;
        private readonly IUserShareRepository _userShareRepository;
        private readonly IIdentityService _identityService;
        private readonly IReportRepository _reportRepository;

        public PostScoringService(ILogger<PostScoringService> logger, IUserReactionRepository userReactionRepository,
            IUserCommentsRepository userCommentsRepository, IUserShareRepository userShareRepository, IIdentityService identityService,
            IReportRepository reportRepository)
        {
            _logger = logger;
            _userReactionRepository = userReactionRepository;
            _userCommentsRepository = userCommentsRepository;
            _userShareRepository = userShareRepository;
            _identityService = identityService;
            _reportRepository = reportRepository;
        }

        private float CalculateDecayFactorAsync(DateTimeOffset createdAt)
        {
            float daysOffset = (DateTimeOffset.Now - createdAt).Days;
            float lambda = 0.05f;
            return (float)Math.Exp(-lambda * daysOffset);
        }

        private async Task<float> CalculateInteractionsScoreAsync(string postId)
        {
            var totalReactionsTask = _userReactionRepository.CountTotalReactionsByPostIdAsync(postId);
            var totalCommentsTask = _userCommentsRepository.CountTotalCommentsByPostIdAsync(postId);
            var totalSharesTask = _userShareRepository.CountTotalSharesByPostIdAsync(postId);
            await Task.WhenAll(totalReactionsTask, totalCommentsTask, totalSharesTask);

            return WeightConstants.Post.Reaction * totalReactionsTask.Result +
                   WeightConstants.Post.Comment * totalCommentsTask.Result +
                   WeightConstants.Post.Share * totalSharesTask.Result;
        }

        private async Task<float> CalculateQualityScoreAsync(string postId)
        {
            var userComments = await _userCommentsRepository.GetUserCommentsByPostId(postId);
            float score = 0f;
            foreach (var userComment in userComments)
            {
                score += userComment.SentimentScore * CalculateDecayFactorAsync(userComment.CreatedAt);
            }
            return score;
        }

        private async Task<float> CalculateReportImpactScoreAsync(string postId)
        {
            var reports = await _reportRepository.GetReportsByPostIdAsync(postId);
            float score = .0f;
            foreach (var report in reports)
            {
                score += report.ReportImpactScore;
            }
            return score;
        }

        public async Task<float> CalculateFinalPostScore(string postId, DateTimeOffset postCreatedAt)
        {
            return await CalculateInteractionsScoreAsync(postId) * CalculateDecayFactorAsync(postCreatedAt) +
                await CalculateQualityScoreAsync(postId) - await CalculateReportImpactScoreAsync(postId);
        }
    }
}
