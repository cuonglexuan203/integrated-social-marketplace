using AutoMapper;
using Feed.Application.DTOs;
using Feed.Core.Entities;

namespace Feed.Application.Mappers.Profiles
{
    public class ReportMappingProfile:Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<Report, ReportDto>().ReverseMap();
        }
    }
}
