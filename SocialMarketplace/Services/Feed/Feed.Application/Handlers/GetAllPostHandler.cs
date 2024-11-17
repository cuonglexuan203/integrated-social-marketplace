using Feed.Application.Mappers;
using Feed.Application.DTOs;
using Feed.Core.Repositories;
using MediatR;
using Feed.Application.Queries;

namespace Feed.Application.Handlers
{
    public class GetAllPostHandler : IRequestHandler<GetAllPostsQuery, IList<PostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IList<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var postList = await _postRepository.GetAllPosts();
            var postResponseList = FeedMapper.Mapper.Map<IList<PostDto>>(postList.ToList());
            return postResponseList;
        }
    }
}
