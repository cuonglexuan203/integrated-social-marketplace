using MediatR;

namespace Feed.Application.Commands
{
    public class RemoveReactionFromPostCommand: IRequest<bool>
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
    }
}
