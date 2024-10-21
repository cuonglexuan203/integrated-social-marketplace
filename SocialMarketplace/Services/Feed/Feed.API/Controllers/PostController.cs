using Feed.Application.Queries;
using Feed.Application.Responses;
using MediatR;
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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<PostResponse>>> GetPosts()
        {
            var query = new GetAllPostsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
