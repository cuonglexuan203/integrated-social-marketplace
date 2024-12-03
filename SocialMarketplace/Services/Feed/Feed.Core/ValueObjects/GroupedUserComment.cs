namespace Feed.Core.ValueObjects
{
    public class GroupedUserComment
    {
        public string UserId { get; set; }
        public string PostId { get; set; }
        public float TotalSentimentScore { get; set; }
        public long Count { get; set; }
    }
}
