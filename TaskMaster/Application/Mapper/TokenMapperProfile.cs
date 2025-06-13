using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapper;

public class TokenMapperProfile: Profile
{
    public TokenMapperProfile()
    {
        CreateMap<RefreshToken, TokenDto>()
            .ForMember(dest => dest.RefreshCreatedAt,
                opt =>
                    opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RefreshExpiresAt,
                opt =>
                    opt.MapFrom(src => src.ExpiresAt))
            .ForMember(dest => dest.IsUsed,
                opt =>
                    opt.MapFrom(src => src.IsUsed))
            .ForMember(dest => dest.RefreshToken,
                opt =>
                    opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.UserId,
                opt =>
                    opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.AccessToken,
                opt =>
                    opt.Ignore())
            .ReverseMap();
        CreateMap<TokenDto, AuthResult>()
            .ForMember(dest => dest.Token,
                opt =>
                    opt.MapFrom(src => src.AccessToken));
    }
}