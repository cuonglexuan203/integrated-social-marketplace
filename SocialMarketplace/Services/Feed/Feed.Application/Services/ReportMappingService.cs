using Feed.Application.DTOs;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Application.Mappers;
using Feed.Core.Entities;
using Feed.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Services
{
    public class ReportMappingService : IReportMappingService
    {
        private readonly ILogger<ReportMappingService> _logger;
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly IPostMappingService _postMappingService;

        public ReportMappingService(
            ILogger<ReportMappingService> logger,
            IPostRepository postRepository,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService,
            IPostMappingService postMappingService
            )
        {
            _logger = logger;
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _postMappingService = postMappingService;
        }
        public async Task<ReportDto> MapToDtoAsync(Report report, CancellationToken token = default)
        {
            // Validate input
            ArgumentNullException.ThrowIfNull(report, nameof(report));

            // Initial mapping
            var reportDto = FeedMapper.Mapper.Map<ReportDto>(report);

            try
            {
                await MapPostAsync(report, reportDto, token);
                await MapTargetUserAsync(report, reportDto, token);
                await MapReporterAsync(report, reportDto, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in mapping report {reportId} to reportDto", report.Id);
            }

            return reportDto;
        }

        private async Task MapPostAsync(Report report, ReportDto reportDto, CancellationToken token)
        {
            if(report.PostId == null)
            {
                _logger.LogError("Cannot map post by empty postId");
                return;
            }
            var post = await _postRepository.GetPostAsync(report.PostId);
            if(post == null)
            {
                _logger.LogError("Cannot map post by not found post id");
                return;
            }
            try
            {
                reportDto.Post = await _postMappingService.MapToDtoAsync(post, token);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in mapping post to postDto {postId}: {errorMsg}", report.PostId, ex.Message);
            }
        }

        private async Task MapTargetUserAsync(Report report, ReportDto reportDto, CancellationToken token)
        {
            try
            {
                var targetUser = await _identityService.GetUserDetailsAsync(report.TargetUserId);
                if (targetUser == null)
                {
                    _logger.LogError("Cannot map target user details for target user id {targetUserId}", report.TargetUserId);
                    return;
                }

                reportDto.TargetUser = FeedMapper.Mapper.Map<UserDto>(targetUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error map target user details for target user id {targetUserId}: {errorMsg}", report.TargetUserId, ex.Message);
            }
        }

        private async Task MapReporterAsync(Report report, ReportDto reportDto, CancellationToken token)
        {
            try
            {
                var reporter = await _identityService.GetUserDetailsAsync(report.ReporterId);
                if (reporter == null)
                {
                    _logger.LogError("Cannot map reporter details for reporter id {reporterId}", report.ReporterId);
                    return;
                }

                reportDto.Reporter = FeedMapper.Mapper.Map<UserDto>(reporter);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error map reporter details for reporter id {reporterId}: {errorMsg}", report.ReporterId, ex.Message);
            }
        }
        public async Task<List<ReportDto>> MapToDtosAsync(IEnumerable<Report> reports, CancellationToken token = default)
        {
            var result = new List<ReportDto>();
            foreach (var report in reports)
            {
                try
                {
                    result.Add(await MapToDtoAsync(report, token));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in mapping report {reportId} to DTO: {errorMsg}", report.Id, ex.Message);
                }
            }
            return result;
        }
    }
}
