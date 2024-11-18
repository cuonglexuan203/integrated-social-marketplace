using Feed.Application.Mappers;
using Feed.Application.DTOs;
using Feed.Core.Repositories;
using MediatR;
using Feed.Application.Queries;
using Microsoft.Extensions.Logging;

namespace Feed.Application.Handlers
{
    public class GetAllPostHandler : IRequestHandler<GetAllPostsQuery, IList<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepo;
        private readonly ILogger<GetAllPostHandler> _logger;

        public GetAllPostHandler(IPostRepository postRepository, ICommentRepository commentRepo, ILogger<GetAllPostHandler> logger)
        {
            _postRepository = postRepository;
            _commentRepo = commentRepo;
            _logger = logger;
        }
        public async Task<IList<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllPosts();
            var postDtos = FeedMapper.Mapper.Map<IList<PostDto>>(posts.ToList());
            foreach (var postDto in postDtos) {
                try
                {
                    var comments = await _commentRepo.GetAllCommentsByPostId(postDto.Id);
                    postDto.Comments = FeedMapper.Mapper.Map<IList<CommentDto>>(comments.ToList());
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "Failed to retrieve comments for post {PostId}. Error: {Error}", postDto.Id, ex.Message);
                }
            }
            return postDtos;
        }
    }
}
