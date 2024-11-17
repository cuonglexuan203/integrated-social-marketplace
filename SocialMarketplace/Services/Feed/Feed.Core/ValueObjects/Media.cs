
namespace Feed.Core.ValueObjects
{
    public class Media
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string Format { get; set; }
        public double? Duration { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }

}
