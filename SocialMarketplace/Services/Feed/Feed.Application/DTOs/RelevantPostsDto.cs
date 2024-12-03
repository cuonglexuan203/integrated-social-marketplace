namespace Feed.Application.DTOs
{
    public class RelevantPostsDto
    {
        public string BasePostId { get; set; }
        public string Keyword { get; set; }
        public IEnumerable<string> RelevantPostIds { get; set; }
    }
}
