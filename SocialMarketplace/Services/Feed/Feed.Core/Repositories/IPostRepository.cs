
using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPost(string id);
        Task<Comment> AddCommentToPostAsync(Comment comment);
        //Task<IEnumerable<Post>> GetPostByUserId(string userId);
        Task<Post> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(string id);
        Task<bool> IsPostExistsAsync(string id);
        Task<Reaction> AddReacionToPostAsync(string postId, Reaction reaction);
    }
}
