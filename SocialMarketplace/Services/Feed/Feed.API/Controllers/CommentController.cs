using Feed.Application.Commands;
using Feed.Application.Common.Models;
using Feed.Application.DTOs;
using Feed.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feed.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    public class CommentController: ApiController
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            ReturnResult<IList<CommentDto>> result = new();
            var query = new GetCommentsByPostIdQuery { PostId = postId };
            result.Result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateComment(CreateCommentCommand command)
        {
            ReturnResult<CommentDto> result = new();
            result.Result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
