using Feed.Core.ValueObjects;
using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;

namespace Feed.Core.Entities
{
    public class Post: AuditableEntity, IIdentifier
    {
        public string Id { get; set; }
        public User User { get; set; }
        public string ContentText { get; set; }
        public List<Media> Media { get; set; }
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; }
        public int CommentsCount { get; set; }
        public List<Comment> Comments { get; set; }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public List<string> Tags { get; set; }
    }
}
