using Feed.Core.Entities;
using Feed.Core.Specs;

namespace Feed.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsByPostId(string postId);
        Task<Pagination<Comment>> GetCommentsByPostId(string postId, CommentSpecParams commentParams);
        Task<Comment> CreateComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(string id);
    }
}
