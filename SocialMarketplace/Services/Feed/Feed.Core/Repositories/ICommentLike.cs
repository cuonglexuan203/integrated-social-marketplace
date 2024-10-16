using Feed.Core.Entities.JoinEntities;

namespace Feed.Core.Repositories
{
    public interface ICommentLike
    {
        Task<IEnumerable<CommentLike>> GetCommentLikes();
        Task<IEnumerable<CommentLike>> GetCommentLikeByUserId(string userId);
        Task<IEnumerable<CommentLike>> GetCommentLikeByCommentId(string commentId);
        Task<bool> CreateCommentLike(CommentLike commentLike);
        //Task<bool> UpdateCommentLike(CommentLike commentLike);
        Task<bool> DeleteCommentLike(string id);
    }
}
