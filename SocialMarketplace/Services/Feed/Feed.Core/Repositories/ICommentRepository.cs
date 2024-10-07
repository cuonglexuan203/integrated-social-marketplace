using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetComments();
        Task<Comment> GetComment(string id);
        Task<IEnumerable<Comment>> GetCommentByPostID(string postId);
        Task<bool> CreateComment(Comment post);
        Task<bool> UpdateComment(Comment post);
        Task<bool> DeleteComment(string id);
    }
}
