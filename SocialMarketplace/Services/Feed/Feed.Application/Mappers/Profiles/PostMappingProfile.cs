using AutoMapper;
using Feed.Application.DTOs;
using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Application.Mappers.Profiles
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Media, MediaDto>().ReverseMap();
        }
    }
}
