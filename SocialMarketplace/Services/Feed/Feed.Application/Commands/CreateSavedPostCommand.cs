using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Commands
{
    public class CreateSavedPostCommand: IRequest<SavedPostDto>
    {
        public string UserId { get; set; } = default!;
        public string PostId { get; set; } = default!;
    }
}
