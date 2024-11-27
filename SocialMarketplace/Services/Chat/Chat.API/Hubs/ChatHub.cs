using Chat.Application.Queries;
using Chat.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IMediator _mediator;

        public ChatHub(ILogger<ChatHub> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Connection {connId} connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public async Task ConnectToRoom(string userId, string targetUserId)
        {
            var room = await _mediator.Send(new GetOrCreateRoomQuery(userId, targetUserId));
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);
            var messageHistory = await _mediator.Send(new GetMessageHistoryQuery(room.Id, new MessageSpecParams()));
            await Clients.Caller.SendAsync("ReceiveMessageHistory", messageHistory);
        }

        //public async Task JoinRoom(string roomId)
        //{
        //    if (await _repository.IsUserInRoomAsync(_currentUser.UserId, roomId))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        //    }
        //}

        //public async Task LeaveRoom(string roomId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        //}

        //public async Task MarkMessageAsRead(string messageId)
        //{
        //    await _repository.UpdateMessageStatusAsync(messageId, MessageStatus.Sent);
        //    await Clients.Group(messageId).SendAsync("MessageRead", messageId);
        //}
    }
}
