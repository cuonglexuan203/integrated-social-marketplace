namespace Feed.Application.DTOs
{
    public class PostDto
    {
        public string Id { get; set; }
        public UserDto User { get; set; }
        public string ContentText { get; set; }
        public List<MediaDto> Media { get; set; }
        public List<ReactionDto> Reactions { get; set; }
        //public IList<CommentDto> Comments { get; set; }
        public string Link { get; set; }
        public string? SharedPostId { get; set; }
        public PostDto? SharedPost { get; set; }
        public List<string> Tags { get; set; }
        public string Category { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
