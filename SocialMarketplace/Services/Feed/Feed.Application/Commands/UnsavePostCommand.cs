
using MediatR;

namespace Feed.Application.Commands
{
    public class UnsavePostCommand:IRequest<bool>
    {
        public string PostId { get; set; }
    }
}
