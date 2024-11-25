using Feed.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Feed.Application.Commands
{
    public class CreatePostCommand: IRequest<PostDto>
    {
        public string UserId { get; set; }
        public string? ContentText { get; set; }
        public IFormFile[]? Files { get; set; }
        public List<string>? Tags { get; set; }
        public string? SharedPostId { get; set; }
    }
}
