
using Feed.Core.Entities.JoinEntities;

namespace Feed.Core.Repositories
{
    public interface IPostTagRepository
    {
        Task<IEnumerable<PostTag>> GetPostTags();
        Task<PostTag> GetPostTag(string id);
        Task<IEnumerable<PostTag>> GetPostTagByTagId(string tagId);
        Task<IEnumerable<PostTag>> GetPostTagByPostId(string postId);
        Task<bool> CreatePostTag(PostTag postTag);
        //Task<bool> UpdatePostTag(PostTag postTag);
        Task<bool> DeletePostTag(string id);
    }
}
