using Feed.Application.Responses;
using MediatR;

namespace Feed.Application.Commands
{
    internal class CreatePostCommand: IRequest<PostResponse>
    {
    }
}
