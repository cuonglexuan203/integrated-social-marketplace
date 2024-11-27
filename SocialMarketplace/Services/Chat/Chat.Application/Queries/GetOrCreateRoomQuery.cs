
using Chat.Application.Dtos;
using Chat.Application.Interfaces.Services;
using MediatR;

namespace Chat.Application.Queries
{
    public class GetOrCreateRoomQuery : IRequest<ChatRoomDto>
    {
        public string UserId { get; set; } = default!;
        public string TargetUserId { get; set; } = default!;
    }

    public class GetOrCreateRoomHandler : IRequestHandler<GetOrCreateRoomQuery, ChatRoomDto>
    {
        private readonly IChatService _chatService;
        private readonly IChatRoomMappingService _chatRoomMappingService;

        public GetOrCreateRoomHandler(IChatService chatService, IChatRoomMappingService chatRoomMappingService)
        {
            _chatService = chatService;
            _chatRoomMappingService = chatRoomMappingService;
        }
        public async Task<ChatRoomDto> Handle(GetOrCreateRoomQuery request, CancellationToken cancellationToken)
        {
            var chatRoom = await _chatService.GetOrCreateRoomAsync(request.UserId, request.TargetUserId, cancellationToken);
            return await _chatRoomMappingService.MapToDtoAsync(chatRoom);
        }
    }
}
