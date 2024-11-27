using Chat.Core.Enums;
using Chat.Core.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _repository;

        public ChatHub(IChatRepository repository)
        {
            _repository = repository;
        }

        //public override async Task OnConnectedAsync()
        //{
        //    var userId = _currentUser.UserId;
        //    var userRooms = await _repository.GetUserRoomsAsync(userId);

        //    foreach (var room in userRooms)
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);
        //    }

        //    await base.OnConnectedAsync();
        //}

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
