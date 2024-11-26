using AutoMapper;
using Identity.Application.DTOs;
using Identity.Infrastructure.Identity;

namespace Identity.Infrastructure.Mappers.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDetailsResponseDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserResponseDTO>().ReverseMap();
        }
    }
}
