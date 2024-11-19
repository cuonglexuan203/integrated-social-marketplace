using Feed.Application.Commands;
using Feed.Application.Common.Models;
using Feed.Application.DTOs;
using Feed.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Feed.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    public class PostController : ApiController
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PostDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<PostDto>>> GetAllPosts()
        {
            ReturnResult<IList<PostDto>> result = new();
            var query = new GetAllPostsQuery();
            result.Result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PostDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostCommand post)
        {
            ReturnResult<PostDto> result = new();
            result.Result = await _mediator.Send(post);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddReaction(AddReactionToPostCommand command)
        {
            ReturnResult<ReactionDto> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]/{postId}")]
        public async Task<IActionResult> GetReactionsByPostId(string postId)
        {
            ReturnResult<IList<ReactionDto>> result = new();
            result.Result = await _mediator.Send(new GetAllReacionsByPostIdQuery() { PostId = postId });
            return Ok(result);
        }
    }
}
