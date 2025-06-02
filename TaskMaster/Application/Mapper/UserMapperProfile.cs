using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapper;

public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Id, 
                opt => 
                    opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, 
                opt => 
                    opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, 
                opt => 
                    opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, 
                opt => 
                    opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhotoUrl, 
                opt => 
                    opt.MapFrom(src => src.PhotoUrl))
            .ReverseMap();
        CreateMap<TelegramUserData, UserDto>()
            .ForMember(dest => dest.Id, 
                opt => 
                    opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, 
                opt => 
                    opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, 
                opt => 
                    opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, 
                opt => 
                    opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhotoUrl, 
                opt => 
                    opt.MapFrom(src => src.PhotoUrl));
    }
}