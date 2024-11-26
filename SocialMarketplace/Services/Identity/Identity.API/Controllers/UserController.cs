using Identity.Application.Commands;
using Identity.Application.Commands.User.Delete;
using Identity.Application.Commands.User.Update;
using Identity.Application.Common.Models;
using Identity.Application.DTOs;
using Identity.Application.Queries;
using Identity.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(List<UserResponseDTO>))]
        public async Task<IActionResult> GetAllUserAsync()
        {
            return Ok(await _mediator.Send(new Application.Queries.User.GetUserQuery()));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Delete/{userId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand() { Id = userId });
            return Ok(result);
        }

        [HttpGet("GetUserDetails/{userId}")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            ReturnResult<UserDetailsResponseDTO> result = new();
            var query = new GetUserDetailsQuery() { UserId = userId };
            result.Result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("GetUserDetailsByUserName/{userName}")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<IActionResult> GetUserDetailsByUserName(string userName)
        {
            ReturnResult<UserDetailsResponseDTO> result = new();
            var query = new GetUserDetailsByUserNameQuery() { UserName = userName };
            result.Result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("AssignRoles")]
        [ProducesDefaultResponseType(typeof(int))]

        public async Task<ActionResult> AssignRoles(AssignUsersRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("EditUserRoles")]
        [ProducesDefaultResponseType(typeof(int))]

        public async Task<ActionResult> EditUserRoles(UpdateUserRolesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetAllUserDetails")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDTO))]
        public async Task<IActionResult> GetAllUserDetails()
        {
            ReturnResult<IList<UserDetailsResponseDTO>> result = new();
            try
            {
                var query = new GetAllUsersDetailsQuery();
                result.Result = await _mediator.Send(query);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("[action]")]
        [ProducesResponseType(typeof(UserDetailsResponseDTO), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> EditUserProfile(EditUserProfileCommand command)
        {
                var result = await _mediator.Send(command);
                return Ok(result);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserFollowers(string userId)
        {
            ReturnResult<IList<UserDetailsResponseDTO>> result = new();
            var query = new GetUserFollowersQuery(userId);
            result.Result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserFollowings(string userId)
        {
            ReturnResult<IList<UserDetailsResponseDTO>> result = new();
            var query = new GetUserFollowingsQuery(userId);
            result.Result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> FollowUser(FollowUserCommand command)
        {
            ReturnResult<bool> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UnfollowUser(UnfollowUserCommand command)
        {
            ReturnResult<bool> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
