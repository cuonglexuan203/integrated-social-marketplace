using Chat.Core.Common.AuditProperties;
using Chat.Core.Entities;
using Chat.Core.Enums;

namespace Chat.Core.ValueObjects
{
    public class Reaction : ICreatedAt
    {
        public User User { get; set; }
        public ReactionType Type { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
    }
}
