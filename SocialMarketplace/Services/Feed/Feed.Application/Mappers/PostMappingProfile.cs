using AutoMapper;
using Feed.Application.Responses;
using Feed.Core.Entities;

namespace Feed.Application.Mappers
{
    public class PostMappingProfile: Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, PostResponse>().ReverseMap();
        }
    }
}
