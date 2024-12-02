using Feed.Application.Commands;
using Feed.Application.Common.Models;
using Feed.Application.DTOs;
using Feed.Application.Queries;
using Feed.Core.Specs;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPosts([FromQuery] PostSpecParams postParams)
        {
            ReturnResult<Pagination<PostDto>> result = new();
            var query = new GetPostsQuery(postParams);
            result.Result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetPost(string postId)
        {
            ReturnResult<PostDto> result = new();
            var query = new GetPostByPostIdQuery(postId);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveReactionFromPost(RemoveReactionFromPostCommand command)
        {
            ReturnResult<bool> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeletePost(DeletePostCommand command)
        {
            ReturnResult<bool> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetAllUserSavedPosts(string userId)
        {
            ReturnResult<IList<SavedPostDto>> result = new();
            var query = new GetAllUserSavedPostsQuery(userId);
            result.Result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSavedPost(CreateSavedPostCommand command)
        {
            ReturnResult<SavedPostDto> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UnsavePost(UnsavePostCommand command)
        {
            ReturnResult<bool> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPostsByUserReaction([FromQuery] ReactionSpecParams reactionParams)
        {
            ReturnResult<Pagination<PostDto>> result = new();
            result.Result = await _mediator.Send(new GetPostsReactedByUserQuery(reactionParams));
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateReport(CreateReportCommand command)
        {
            ReturnResult<ReportDto> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchPosts([FromQuery] PostSpecParams postSpecParams)
        {
            ReturnResult<IEnumerable<PostDto>> result = new();
            var command = new SearchPostsCommand(postSpecParams);
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
