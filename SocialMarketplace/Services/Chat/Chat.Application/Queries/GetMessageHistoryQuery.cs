
using Chat.Application.Dtos;
using Chat.Application.Extensions;
using Chat.Application.Interfaces.Services;
using Chat.Core.Entities;
using Chat.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Queries
{
    public class GetMessageHistoryQuery: IRequest<Pagination<MessageDto>>
    {
        public GetMessageHistoryQuery(string roomId, MessageSpecParams messageSpecParams)
        {
            RoomId = roomId;
            MessageSpecParams = messageSpecParams;
        }

        public string RoomId { get; set; } = default!;
        public MessageSpecParams MessageSpecParams { get; set; }
    }

    public class GetMessageHistoryHandler : IRequestHandler<GetMessageHistoryQuery, Pagination<MessageDto>>
    {
        private readonly ILogger<GetOrCreateRoomHandler> _logger;
        private readonly IChatService _chatService;

        public GetMessageHistoryHandler(ILogger<GetOrCreateRoomHandler> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }
        public async Task<Pagination<MessageDto>> Handle(GetMessageHistoryQuery request, CancellationToken cancellationToken)
        {
            var messagePage = await _chatService.GetMessageHistoryAsync(request.RoomId, request.MessageSpecParams, cancellationToken);
            return messagePage.Map<Message, MessageDto>();

        }
    }
}
