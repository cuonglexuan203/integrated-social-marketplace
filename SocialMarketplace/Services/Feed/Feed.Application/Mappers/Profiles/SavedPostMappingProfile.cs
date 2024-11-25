
using AutoMapper;
using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Mappers.Profiles
{
    public class SavedPostMappingProfile: Profile
    {
        public SavedPostMappingProfile()
        {
            CreateMap<Post, SavedPostDto>().ReverseMap();
        }
    }
}
