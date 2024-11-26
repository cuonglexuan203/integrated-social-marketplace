
using AutoMapper;
using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Mappers.Profiles
{
    internal class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
