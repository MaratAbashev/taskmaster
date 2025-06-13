using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapper;

public class InfrastructureMapperProfile: Profile
{
    public InfrastructureMapperProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.NameToPing,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.RefreshTokens,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.SocialRating, 
                opt => 
                    opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.SocialRating, 
                opt => 
                    opt.MapFrom(src => src.SocialRating));
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
            .ForMember(dest => dest.FinishedAt,
                opt =>
                    opt.MapFrom(src => src.FinishedAt))
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
            .ForMember(dest => dest.PriorityLevel,
                opt =>
                    opt.MapFrom(src => src.PriorityLevel))
            .ForMember(dest => dest.CreationDate,
                opt =>
                    opt.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.DueDate,
                opt =>
                    opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.ApprovingDate,
                opt =>
                    opt.MapFrom(src => src.ApprovingDate))
            .ForMember(dest => dest.SentToApproveDate,
                opt =>
                    opt.MapFrom(src => src.SentToApproveDate))
            .ForMember(dest => dest.BoardId,
                opt =>
                    opt.MapFrom(src => src.BoardId))
            .ForMember(dest => dest.TaskWorkers,
                opt =>
                    opt.MapFrom(src => src.TaskWorkers))
            .ForMember(dest => dest.Author,
                opt =>
                    opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.Leader,
            opt =>
                opt.MapFrom(src => src.Leader))
            .ForMember(dest => dest.LeaderId,
                opt =>
                    opt.MapFrom(src => src.LeaderId))
            .ForMember(dest => dest.AuthorId,
                opt =>
                    opt.MapFrom(src => src.AuthorId))
            .ReverseMap()
            .ForMember(dest => dest.TaskWorkers,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.Author,
                opt =>
                    opt.Ignore())
            .ForMember(dest => dest.Leader,
                opt =>
                    opt.Ignore());
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
    }
}