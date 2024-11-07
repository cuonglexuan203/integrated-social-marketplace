using Feed.Application.Mappers;
using Feed.Application.Queries;
using Feed.Application.Responses;
using Feed.Core.Repositories;
using MediatR;

namespace Feed.Application.Handlers
{
    public class GetAllPostHandler : IRequestHandler<GetAllPostsQuery, IList<PostResponse>>
    {
        private readonly IPostRepository _postRepository;

        public GetAllPostHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IList<PostResponse>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var postList = await _postRepository.GetPosts();
            var postResponseList = FeedMapper.Mapper.Map<IList<PostResponse>>(postList.ToList());
            return postResponseList;
        }
    }
}
