using Feed.Core.Common.AuditProperties;
using Feed.Core.Entities;
using Feed.Core.Enums;

namespace Feed.Core.ValueObjects
{
    public class Reaction : ICreatedAt
    {
        public User User { get; set; }
        public ReactionType Type { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
    }

}
