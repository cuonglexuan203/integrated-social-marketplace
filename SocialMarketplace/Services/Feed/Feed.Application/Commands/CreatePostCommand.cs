using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Commands
{
    public class CreatePostCommand: IRequest<PostResponse>
    {
    }
}
