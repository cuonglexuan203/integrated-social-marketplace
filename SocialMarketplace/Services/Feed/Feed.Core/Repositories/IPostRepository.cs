
using Feed.Core.Entities;
using Feed.Core.Interfaces;
using Feed.Core.Specs;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface IPostRepository: ISoftDeletable<Post>
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Pagination<Post>> GetPostsAsync(PostSpecParams postParams, CancellationToken token = default);
        Task<Post> GetPostAsync(string id, CancellationToken token = default);
        Task<Comment> AddCommentToPostAsync(Comment comment);
        //Task<IEnumerable<Post>> GetPostByUserId(string userId);
        Task<Post> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        //Task<bool> DeletePost(string id);
        Task<bool> IsPostExistsAsync(string id);
        Task<Reaction> AddReacionToPostAsync(string postId, Reaction reaction, CancellationToken cancellationToken = default);
        Task<bool> RemoveReactionFromPostAsync(string postId, string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Reaction>> GetAllReactionsByPostId(string postId, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string postId, CancellationToken token = default);
    }
}
