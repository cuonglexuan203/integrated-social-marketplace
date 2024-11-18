
using Feed.Application.DTOs;
using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Core.Repositories;
using MediatR;

namespace Feed.Application.Handlers
{
    public class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostIdQuery, IList<CommentDto>>
    {
        private readonly ICommentRepository _commentRepo;

        public GetCommentsByPostIdHandler(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }
        public async Task<IList<CommentDto>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepo.GetAllCommentsByPostId(request.PostId);
            return FeedMapper.Mapper.Map<IList<CommentDto>>(comments);
        }
    }
}
