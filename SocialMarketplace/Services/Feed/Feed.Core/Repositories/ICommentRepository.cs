using Feed.Core.Entities;
using Feed.Core.Specs;

namespace Feed.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<Pagination<Comment>> GetComments(CommentSpecParams commentParams);
        Task<Comment> GetComment(string id);
        Task<IEnumerable<Comment>> GetCommentByPostID(string postId);
        Task<bool> CreateComment(Comment post);
        Task<bool> UpdateComment(Comment post);
        Task<bool> DeleteComment(string id);
    }
}
