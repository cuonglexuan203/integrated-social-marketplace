
using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Commands
{
    public class AddReactionToPostCommand: IRequest<ReactionDto>
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
        public int ReactionType { get; set; }
    }
}
