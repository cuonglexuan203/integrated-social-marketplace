using Feed.Core.Entities;

namespace Feed.Application.DTOs
{
    public class PostDto
    {
        public string Id { get; set; }
        public CompactUser User { get; set; }
        public string ContentText { get; set; }
        public List<MediaDto> Media { get; set; }
        public int LikesCount { get; set; }
        public List<ReactionDto> Reactions { get; set; }
        public int CommentsCount { get; set; }
        public IList<CommentDto> Comments { get; set; }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public List<string> Tags { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
