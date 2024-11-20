
using MediatR;

namespace Feed.Application.Commands
{
    public class DeleteCommentCommand: IRequest<bool>
    {
        public string CommentId { get; set; } = default!;
    }
}
