
using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Application.DTOs
{
    public class PostResponse
    {
        public string Id { get; set; }
        public string UserID { get; set; }
        public string ContentText { get; set; }
        public List<Media> Media { get; set; }
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; }
        public int CommentsCount { get; set; }
        public List<Comment> Comments { get; set; }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public List<string> Tags { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
