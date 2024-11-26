using Feed.Core.Entities;

namespace Feed.Application.DTOs
{
    public class SavedPostDto
    {
        public string Id { get; set; }
        public User User { get; set; }
        public string ContentText { get; set; }
        public List<MediaDto> Media { get; set; }
        public List<ReactionDto> Reactions { get; set; }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public PostDto? SharedPost { get; set; }
        public List<string> Tags { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset? SavedAt { get; set; }
    }
}
