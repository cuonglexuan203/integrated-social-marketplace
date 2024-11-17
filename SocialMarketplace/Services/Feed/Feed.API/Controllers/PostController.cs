using Feed.Application.Commands;
using Feed.Application.DTOs;
using Feed.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Feed.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize]
    public class PostController: ApiController
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
            var query = new GetAllPostsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePost(CreatePostCommand post)
        {
            var result = await _mediator.Send(post);
            return Ok(result);
        }
    }
}
