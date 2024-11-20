using MediatR;

namespace Feed.Application.Commands
{
    public class RemoveReactionFromCommentCommand: IRequest<bool>
    {
        public string CommentId { get; set; }
        public string UserId { get; set; }
    }
}
