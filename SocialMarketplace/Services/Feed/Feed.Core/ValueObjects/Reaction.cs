using Feed.Core.Common.AuditProperties;
using Feed.Core.Enums;

namespace Feed.Core.ValueObjects
{
    public class Reaction : ICreatedAt
    {
        public string UserId { get; set; }
        public ReactionType Type { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }

}
