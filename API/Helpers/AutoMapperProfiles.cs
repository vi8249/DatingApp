using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile // map one object to another
    {
        public AutoMapperProfiles()
        {
            //<from, to>
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opts => opts.MapFrom(
                    src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
        }
    }
}