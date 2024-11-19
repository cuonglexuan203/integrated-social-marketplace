using Feed.Core.Entities;
using Feed.Core.Enums;

namespace Feed.Application.DTOs
{
    public class ReactionDto
    {
        public CompactUser User { get; set; }
        public ReactionType Type { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
