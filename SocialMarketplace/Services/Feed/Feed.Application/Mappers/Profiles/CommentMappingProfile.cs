using AutoMapper;
using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Mappers.Profiles
{
    internal class CommentMappingProfile:Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDto>().ReverseMap();
        }
    }
}
