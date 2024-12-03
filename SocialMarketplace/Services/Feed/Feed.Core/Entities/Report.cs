using Feed.Core.Common.AuditProperties;
using Feed.Core.Common.BaseEntities;
using Feed.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Feed.Core.Entities
{
    public class Report : AuditableEntity, IIdentifier
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string PostId { get; set; }
        public string TargetUserId { get; set; }
        public string ReporterId { get; set; }
        public ReportType ReportType { get; set; }
        public string ContentText { get; set; }
        public bool? Validity { get; set; }
        public float ReportImpactScore { get; set; } // severity weight * reporter credibility

        public float CalculateReportImpactScore(float reporterCredibilityScore)
        {
            ReportImpactScore = reporterCredibilityScore * ReportType switch
            {
                ReportType.Spam => 1,
                ReportType.Harassment => 2,
                ReportType.HateSpeech => 3,
                ReportType.Scam => 3,
                ReportType.Fake => 3,
                _ => 0
            };
            return ReportImpactScore;
        }
    }
}
