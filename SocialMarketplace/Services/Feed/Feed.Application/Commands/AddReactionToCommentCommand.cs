using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Commands
{
    public class AddReactionToCommentCommand : IRequest<ReactionDto>
    {
        public string CommentId { get; set; }
        public string UserId { get; set; }
        public int ReactionType { get; set; }
    }
}
