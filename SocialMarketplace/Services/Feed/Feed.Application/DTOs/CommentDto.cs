namespace Feed.Application.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public UserDto User { get; set; }
        public List<MediaDto> Media { get; set; }
        public string CommentText { get; set; }
        public List<ReactionDto> Reactions { get; set; }
        public string? ParentCommentID { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
