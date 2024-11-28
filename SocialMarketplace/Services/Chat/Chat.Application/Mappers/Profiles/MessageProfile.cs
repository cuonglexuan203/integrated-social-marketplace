
using AutoMapper;
using Chat.Application.Dtos;
using Chat.Core.Entities;
using Chat.Core.ValueObjects;

namespace Chat.Application.Mappers.Profiles
{
    public class MessageProfile: Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<Media, MediaDto>().ReverseMap();
            CreateMap<Reaction, ReactionDto>().ReverseMap();
            CreateMap<MessageReadInfo, MessageReadInfoDto>().ReverseMap();
            CreateMap<PostReference, PostReferenceDto>().ReverseMap();
        }
    }
}
