﻿using Chat.Application.Dtos;
using Chat.Application.Extensions;
using Chat.Application.Interfaces.Services;
using Chat.Application.Mappers;
using Chat.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Queries
{
    public class GetUserRoomsQuery: IRequest<IList<ChatRoomDto>>
    {
        public string UserId { get; set; }

        public GetUserRoomsQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserRoomsHandler : IRequestHandler<GetUserRoomsQuery, IList<ChatRoomDto>>
    {
        private readonly ILogger<GetUserRoomsHandler> _logger;
        private readonly IChatService _chatService;

        public GetUserRoomsHandler(ILogger<GetUserRoomsHandler> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }
        public async Task<IList<ChatRoomDto>> Handle(GetUserRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _chatService.GetUserRoomsAsync(request.UserId, cancellationToken);
            var roomDtos = ChatMapper.Mapper.Map<List<ChatRoomDto>>(rooms);
            foreach (var roomDto in roomDtos)
            {
                var messagePage = await _chatService.GetMessageHistoryAsync(roomDto.Id, new Core.Specs.MessageSpecParams
                {
                    PageSize = 1
                });

                roomDto.MessagePage = messagePage.Map<Message, MessageDto>();
            }

            return roomDtos;
        }
    }
}