using AutoMapper;
using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Mappers.Profiles
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostResponse>().ReverseMap();
        }
    }
}
