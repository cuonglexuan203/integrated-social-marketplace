using Feed.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Feed.Application.Commands
{
    public class CreateCommentCommand: IRequest<CommentDto>
    {
        public string PostId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public IFormFile? Media { get; set; }
        public string? CommentText { get; set; }
    }
}
