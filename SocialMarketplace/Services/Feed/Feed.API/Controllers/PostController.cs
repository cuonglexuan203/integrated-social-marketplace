using Feed.Application.DTOs;
using Feed.Application.Queries.Post;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Feed.API.Controllers
{
    public class PostController: ApiController
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<PostResponse>>> GetAllPosts()
        {
            var query = new GetAllPostsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
