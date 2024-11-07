using Feed.Core.Entities.AuditProperties;
using Feed.Core.Entities.Enums;

namespace Feed.Core.ValueObjects
{
    public class Reaction : ICreatedAt
    {
        public string UserId { get; set; }
        public ReactionType Type { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }

}
