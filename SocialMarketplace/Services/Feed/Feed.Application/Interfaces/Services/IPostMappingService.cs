
using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Interfaces.Services
{
    public interface IPostMappingService
    {
        Task<PostDto> MapToDtoAsync(Post post, CancellationToken token = default);
        Task<List<PostDto>> MapToDtosAsync(IEnumerable<Post> posts, CancellationToken token = default);
        // Saved post
        Task<SavedPostDto> MapToDtoAsync(SavedPost savedPost, CancellationToken token = default);
        Task<List<SavedPostDto>> MapToDtosAsync(IEnumerable<SavedPost> posts, CancellationToken token = default);
    }
}
