using Chat.Application.Dtos;

namespace Chat.Application.Interfaces.HttpClients
{
    public interface IFeedService
    {
        Task<PostReferenceDto> GetPostAsync(string postId, CancellationToken cancellationToken = default);
    }
}
