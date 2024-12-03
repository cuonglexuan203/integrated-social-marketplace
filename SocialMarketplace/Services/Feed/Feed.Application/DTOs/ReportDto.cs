using Feed.Core.Enums;

namespace Feed.Application.DTOs
{
    public class ReportDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public PostDto Post { get; set; }
        public string TargetUserId { get; set; }
        public UserDto TargetUser { get; set; }
        public string ReporterId { get; set; }
        public UserDto Reporter { get; set; }
        public ReportType ReportType { get; set; }
        public string ContentText { get; set; }
        public bool? Validity { get; set; }
        public float ReportImpactScore { get; set; }
    }
}
