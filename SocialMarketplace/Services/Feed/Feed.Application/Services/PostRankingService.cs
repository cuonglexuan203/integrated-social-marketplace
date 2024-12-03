using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Services
{
    public class PostRankingService : IPostRankingService
    {
        private readonly ILogger<PostRankingService> _logger;
        private readonly IUserReactionRepository _userReactionRepo;
        private readonly IUserCommentsRepository _userCommentsRepo;
        private readonly IUserShareRepository _userShareRepo;
        private readonly IPostRepository _postRepo;
        private readonly IRecommendationService _recommendationService;
        private readonly IPostMappingService _postMappingService;
        private const float _userReactionFactor = 1;
        private const float _userCommentFactor = 3;
        private const float _userShareFactor = 5;

        private const float _userFollowingFactor = 0.5f;

        public PostRankingService(ILogger<PostRankingService> logger, IUserReactionRepository userReactionRepo,
            IUserCommentsRepository userCommentsRepo, IUserShareRepository userShareRepo, IPostRepository postRepo,
            IRecommendationService recommendationService, IPostMappingService postMappingService)
        {
            _logger = logger;
            _userReactionRepo = userReactionRepo;
            _userCommentsRepo = userCommentsRepo;
            _userShareRepo = userShareRepo;
            _postRepo = postRepo;
            _recommendationService = recommendationService;
            _postMappingService = postMappingService;
        }
        public async Task<IEnumerable<string>> GetTopPostIdsByUserInteractionAsync(string userId, int take)
        {
            var groupedUserReactions = (await _userReactionRepo.GroupUserReactionByUserIdAsync(userId)).ToList();
            var groupedUserComments = (await _userCommentsRepo.GroupUserCommentByUserIdAsync(userId)).ToList();
            var groupedUserShares = (await _userShareRepo.GroupUserShareByUserIdAsync(userId)).ToList();

            if (groupedUserReactions.Count == 0 && groupedUserComments.Count == 0 && groupedUserShares.Count == 0)
            {
                return await _postRepo.GetTopPostIdsByScoreAsync(take);
            }

            var postScoreDic = new Dictionary<string, float>();

            foreach (var g in groupedUserReactions)
            {
                if (!postScoreDic.ContainsKey(g.PostId))
                    postScoreDic[g.PostId] = 0f;
                postScoreDic[g.PostId] += g.Count * _userReactionFactor;
            }

            foreach (var g in groupedUserComments)
            {
                if (!postScoreDic.ContainsKey(g.PostId))
                    postScoreDic[g.PostId] = 0f;
                postScoreDic[g.PostId] += g.TotalSentimentScore * _userCommentFactor;
            }

            foreach (var g in groupedUserShares)
            {
                if (!postScoreDic.ContainsKey(g.PostId))
                    postScoreDic[g.PostId] = 0f;
                postScoreDic[g.PostId] += g.Count * _userShareFactor;
            }

            var result = postScoreDic.OrderByDescending(x => x.Value).Take(take).Select(x => x.Key).ToList();

            if (result.Count < take)
            {
                result.AddRange(await _postRepo.GetTopPostIdsByScoreAsync(take - result.Count));
            }

            return result;
        }
        public async Task<IEnumerable<PostDto>> GetRecommendedPostsAsync(string userId, int skip, int take)
        {
            var postIds = await GetTopPostIdsByUserInteractionAsync(userId, take);
            var recommendedPostIdSet = new HashSet<string>();
            foreach (var postId in postIds)
            {
                try
                {
                    var tempRelevantPostIds = (await _recommendationService.GetRelevantPostIdsAsync(postId, skip, take)).ToList();
                    foreach (var relevantPostId in tempRelevantPostIds)
                    {
                        recommendedPostIdSet.Add(relevantPostId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in getting relevant posts of postId {postId}: {errorMsg}", postId, ex.Message);
                }
            }
            var recommendedPostIds = recommendedPostIdSet.ToList();
            var recommendedPosts = new List<Post>();
            foreach (var postId in recommendedPostIds)
            {
                try
                {
                    var tempPost = await _postRepo.GetPostAsync(postId);
                    if (tempPost == null)
                    {
                        _logger.LogWarning("Getting post by post id {postId} not found", postId);
                        continue;
                    }
                    recommendedPosts.Add(tempPost);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in getting post by post id {postId}: {errorMsg}", postId, ex.Message);
                }
            }
            var recommendedPostDtos = await _postMappingService.MapToDtosAsync(recommendedPosts);
            var orderedRecommendedPostDtos  = recommendedPostDtos.OrderByDescending(
                x => x.FinalPostScore * (1 + (x.User?.IsFollowing == true ? _userFollowingFactor : 0)));
            return orderedRecommendedPostDtos.Take(take).ToList();
        }
    }
}
