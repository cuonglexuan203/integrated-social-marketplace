using Feed.Core.Entities;

namespace Feed.Core.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTags();
        Task<Tag> GetTag(string id);
        Task<Tag> GetTagByName(string tagName);
        Task<bool> CreateTag(Tag tag);
        Task<bool> UpdateTag(Tag tag);
        Task<bool> DeleteTag(string id);
    }
}
