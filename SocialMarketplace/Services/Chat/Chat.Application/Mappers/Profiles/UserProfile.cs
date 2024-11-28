using AutoMapper;
using Chat.Application.Dtos;
using Chat.Core.Entities;

namespace Chat.Application.Mappers.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
