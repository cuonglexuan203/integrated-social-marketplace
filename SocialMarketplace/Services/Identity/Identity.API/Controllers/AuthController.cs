using Identity.Application.Commands.Auth;
using Identity.Application.Common.Models;
using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using MediatR;
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
            try
            {
                result.Result = await _mediator.Send(command);
            }
            catch(BadRequestException ex)
            {
                result.Message = ex.Message;
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
