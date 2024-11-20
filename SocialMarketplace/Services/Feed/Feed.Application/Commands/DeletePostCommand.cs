using MediatR;

namespace Feed.Application.Commands
{
    public class DeletePostCommand: IRequest<bool>
    {
        public string PostId { get; set; } = default!;
    }
}
