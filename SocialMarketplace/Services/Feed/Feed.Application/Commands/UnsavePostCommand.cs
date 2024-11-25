
using MediatR;

namespace Feed.Application.Commands
{
    public class UnsavePostCommand:IRequest<bool>
    {
        public string UserId { get; set; }
        public string PostId { get; set; }
    }
}
