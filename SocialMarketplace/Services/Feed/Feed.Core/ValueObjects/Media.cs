using Feed.Core.Enums;

namespace Feed.Core.ValueObjects
{
    public class Media
    {
        public string Url { get; set; }                       
        public MediaType Type { get; set; }                     
        public string? ThumbnailUrl { get; set; }             
        public TimeSpan? Duration { get; set; }
    }

}
