using Feed.Core.Entities;
using Feed.Core.Interfaces;
using Feed.Core.Specs;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface ICommentRepository : ISoftDeletable<Comment>
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<IEnumerable<Comment>> GetAllCommentsByPostId(string postId);
        Task<Pagination<Comment>> GetCommentsByPostId(string postId, CommentSpecParams commentParams);
        Task<Comment> CreateComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(string id);
        Task<Reaction> AddReacionToCommentAsync(string commentId, Reaction reaction, CancellationToken cancellationToken = default);
        Task<bool> RemoveReactionFromCommentAsync(string commentId, string userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string commentId, CancellationToken token = default);
        Task<Pagination<Comment>> GetCommentsByUserIdAsync(string userId, CommentSpecParams commentParams, CancellationToken cancellationToken = default);
    }
}
