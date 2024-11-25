using AutoMapper;
using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Dto;

namespace VideoGamesCatalog.Server.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<GenreDto, Genre>().ReverseMap();
           
            CreateMap<VideoGameDto, VideoGame>();
            CreateMap<VideoGame, VideoGameDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.ImageUrl))
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => src.Platform.Name));

            CreateMap<PagedResult<VideoGame>, PagedResult<VideoGameDto>>().ReverseMap();

            CreateMap<AgeRatingDto, AgeRating>().ReverseMap();
            CreateMap<PlatformDto, Platform>().ReverseMap();
            CreateMap<VideoGameImageDto, VideoGameImage>().ReverseMap();
        }
    }
}
