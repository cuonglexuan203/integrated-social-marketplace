using Chat.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    public class ChatController: ApiController
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserRooms(string userId)
        {
            var rooms = await _mediator.Send(new GetUserRoomsQuery(userId));
            return Ok(rooms);
        }
    }
}
