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
            ReturnResult<IList<PostDto>> result = new();
            try
            {
                var query = new GetAllPostsQuery();
                result.Result = await _mediator.Send(query);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PostDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePost(CreatePostCommand post)
        {
            ReturnResult<PostDto> result = new();
            try
            {
                result.Result = await _mediator.Send(post);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }
    }
}
