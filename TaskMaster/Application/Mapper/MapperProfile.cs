using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapper;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.NameToPing,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.RefreshTokens,
                opt =>
                    opt.Ignore())
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
        CreateMap<BoardUser, UserDto>()
            .IncludeMembers(src => src.User)
            .ForMember(dest => dest.NameToPing,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.Id, 
                opt => 
                    opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.IsOwner,
                opt =>
                    opt.MapFrom(src => src.IsOwner))
            .ForMember(dest => dest.CanCreateTasks,
                opt =>
                    opt.MapFrom(src => src.CanCreateTasks))
            .ForMember(dest => dest.BoardId,
                opt =>
                    opt.MapFrom(src => src.BoardId));
        CreateMap<TaskWorker, UserDto>()
            .IncludeMembers(src => src.User)
            .ForMember(dest => dest.NameToPing,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.Id,
                opt =>
                    opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.StartedAt,
                opt =>
                    opt.MapFrom(src => src.StartedAt))
            .ForMember(dest => dest.IsConfirmed,
                opt =>
                    opt.MapFrom(src => src.IsConfirmed));
        CreateMap<ToDoTask, TaskDto>()
            .ForMember(dest => dest.Id,
                opt =>
                    opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,
                opt =>
                    opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description,
                opt =>
                    opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status,
                opt =>
                    opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreationDate,
                opt =>
                    opt.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.DueDate,
                opt =>
                    opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.CompletingDate,
                opt =>
                    opt.MapFrom(src => src.CompletingDate))
            .ForMember(dest => dest.BoardId,
                opt =>
                    opt.MapFrom(src => src.BoardId))
            .ForMember(dest => dest.TaskWorkers,
                opt =>
                    opt.MapFrom(src => src.TaskWorkers))
            .ForMember(dest => dest.Author,
                opt =>
                    opt.MapFrom(src => src.Author));
        CreateMap<TaskBoard, TaskBoardDto>()
            .ForMember(dest => dest.Id,
                opt =>
                    opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,
                opt =>
                    opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Users,
                opt =>
                    opt.MapFrom(src => src.BoardUsers))
            .ForMember(dest => dest.Tasks,
                opt =>
                    opt.MapFrom(src => src.BoardTasks))
            .ForMember(dest => dest.TelegramGroupLink,
                opt =>
                    opt.MapFrom(src => src.Group != null ? src.Group.Link : null));
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