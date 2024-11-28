using Chat.Application.Dtos;
using Chat.Application.Interfaces.HttpClients;
using Chat.Application.Interfaces.Services;
using Chat.Application.Mappers;
using Chat.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services
{
    public class ChatRoomMappingService : IChatRoomMappingService
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ChatRoomMappingService> _logger;

        public ChatRoomMappingService(IIdentityService identityService, ILogger<ChatRoomMappingService> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }
        public async Task<ChatRoomDto> MapToDtoAsync(ChatRoom room, CancellationToken token = default)
        {
            var roomDto = ChatMapper.Mapper.Map<ChatRoomDto>(room);

            await MapParticipantsAsync(room, roomDto, token);

            return roomDto;
        }

        private async Task MapParticipantsAsync(ChatRoom room, ChatRoomDto roomDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var userDtos = new List<UserDto>();
                foreach (var userId in room.ParticipantIds)
                {
                    try
                    {
                        var user = await _identityService.GetUserDetailsAsync(userId, cancellationToken);
                        if (user == null)
                        {
                            _logger.LogWarning("Cannot get user details {userId} of the pariticipant", userId);
                            continue;
                        }
                        userDtos.Add(ChatMapper.Mapper.Map<UserDto>(user));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Error in retrieving the chat room participant - user id {userId}: {errorMsg}", userId, e.Message);
                    }
                }
                roomDto.Participants = userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in retrieving the participants: {errorMsg}", ex.Message);
            }
        }
    }
}
