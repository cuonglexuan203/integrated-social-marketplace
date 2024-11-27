using AutoMapper;
using Chat.Application.Dtos;
using Chat.Core.Entities;

namespace Chat.Application.Mappers.Profiles
{
    public class ChatRoomProfile: Profile
    {
        public ChatRoomProfile()
        {
            CreateMap<ChatRoom, ChatRoomDto>().ReverseMap();
        }
    }
}
