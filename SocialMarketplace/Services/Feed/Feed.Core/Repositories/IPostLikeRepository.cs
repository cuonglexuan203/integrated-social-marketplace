
using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface IPostLikeRepository
    {
        Task<IEnumerable<PostLike>> GetPostLikes();
        Task<IEnumerable<PostLike>> GetPostLikeByUserId(string userId);
        Task<IEnumerable<PostLike>> GetPostLikeByPostId(string postId);
        Task<bool> CreatePostLike(PostLike postLike);
        //Task<bool> UpdatePostLike(PostLike postLike);
        Task<bool> DeletePostLike(string id);
    }
}
