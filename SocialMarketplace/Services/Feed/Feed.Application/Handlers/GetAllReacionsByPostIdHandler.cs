
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetAllReacionsByPostIdHandler : IRequestHandler<GetAllReacionsByPostIdQuery, IList<ReactionDto>>
    {
        private readonly ILogger<GetAllReacionsByPostIdQuery> _logger;
        private readonly IPostRepository _postRepo;

        public GetAllReacionsByPostIdHandler(ILogger<GetAllReacionsByPostIdQuery> logger, IPostRepository postRepo)
        {
            _logger = logger;
            _postRepo = postRepo;
        }
        public async Task<IList<ReactionDto>> Handle(GetAllReacionsByPostIdQuery request, CancellationToken cancellationToken)
        {
            var reactions = await _postRepo.GetAllReactionsByPostId(request.PostId);
            return FeedMapper.Mapper.Map<IList<ReactionDto>>(reactions.ToList());
            
        }
    }
}
