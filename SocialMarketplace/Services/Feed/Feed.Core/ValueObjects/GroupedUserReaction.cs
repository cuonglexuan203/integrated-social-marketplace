namespace Feed.Core.ValueObjects
{
    public class GroupedUserReaction
    {
        public string UserId { get; set; }
        public string PostId { get; set; }
        public long Count { get; set; }
    }
}
