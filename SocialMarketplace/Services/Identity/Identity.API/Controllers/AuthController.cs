using Identity.Application.Commands.Auth;
using Identity.Application.Commands.User.Create;
using Identity.Application.Common.Models;
using Identity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.API.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResponseDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthCommand command)
        {
            ReturnResult<AuthResponseDTO> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Logout")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            ReturnResult<Unit> result = new();
            var command = new LogoutCommand();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> Register(CreateUserCommand command)
        {
            ReturnResult<int> result = new();
            result.Result = await _mediator.Send(command);

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            ReturnResult<bool> result = new();
            result.Result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
