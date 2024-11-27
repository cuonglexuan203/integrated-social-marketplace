
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
            CreateMap<Message, MessageDto>();
            CreateMap<Media, MediaDto>();
            CreateMap<Reaction, ReactionDto>();
            CreateMap<MessageReadInfo, MessageReadInfoDto>();
            CreateMap<PostReference, PostReferenceDto>();
        }
    }
}
