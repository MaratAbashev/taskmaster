using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Mapper;

public class TaskBoardMapperProfile: Profile
{
    public TaskBoardMapperProfile()
    {
        CreateMap<TaskBoardDto, TaskBoardResponse>()
            .ForMember(dest => dest.Users, 
                opt => 
                    opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.Tasks, 
                opt => 
                    opt.MapFrom(src => src.Tasks));
        
        CreateMap<UserDto, TaskBoardUserResponse>()
                .ForMember(dest => dest.DisplayName, 
                    opt => 
                        opt.MapFrom(src => 
                            string.IsNullOrEmpty(src.Username) 
                                ? $"{src.FirstName} {src.LastName}".Trim() 
                                : $"@{src.Username}")) 
                .ForMember(dest => dest.IsOwner, 
                opt => 
                    opt.MapFrom(src => src.IsOwner ?? false)) 
                .ForMember(dest => dest.CanCreateTasks, 
                opt => 
                    opt.MapFrom(src => src.CanCreateTasks ?? false)) 
                .ForMember(dest => dest.SocialRating, 
                opt => 
                    opt.MapFrom(src => src.SocialRating ?? 0));

        CreateMap<UserDto, TaskUserResponse>()
            .ForMember(dest => dest.Id, 
            opt => 
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DisplayName, 
            opt => 
                opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Username)
                    ? $"{src.FirstName} {src.LastName}".Trim()
                    : $"@{src.Username}"))
            .ForMember(dest => dest.PhotoUrl, 
            opt => 
                opt.MapFrom(src => src.PhotoUrl))
            .ForMember(dest => dest.SocialRating, 
            opt => 
                opt.MapFrom(src => src.SocialRating ?? 0))
            .ForMember(dest => dest.IsConfirmed, 
            opt => 
                opt.MapFrom(src => src.IsConfirmed))
            .ForMember(dest => dest.StartedAt,
                opt => 
                    opt.MapFrom(src => src.StartedAt))
            .ForMember(dest => dest.FinishedAt,
                opt => 
                    opt.MapFrom(src => src.FinishedAt));
        
        CreateMap<TaskDto, TaskResponse>()
            .ForMember(dest => dest.Title,
                opt => 
                    opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, 
            opt => 
                opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status, 
            opt => 
                opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DueDate, 
            opt => 
                opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.SentToApproveDate, 
                opt => 
                    opt.MapFrom(src => src.SentToApproveDate))
            .ForMember(dest => dest.ApprovingDate, 
                opt => 
                    opt.MapFrom(src => src.ApprovingDate))
            .ForMember(dest => dest.PriorityLevel, 
            opt => 
                opt.MapFrom(src => src.PriorityLevel))
            .ForMember(dest => dest.Author, 
            opt => 
                opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.Leader, 
            opt => 
                opt.MapFrom(src => src.Leader))
            .ForMember(dest => dest.Workers, 
            opt => 
                opt.MapFrom(src => src.TaskWorkers));
        CreateMap<CreateTaskRequest, TaskDto>()
            .ForMember(dest => dest.Title, 
            opt => 
                opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, 
            opt => 
                opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PriorityLevel, 
            opt => 
                opt.MapFrom(src => src.PriorityLevel))
            .ForMember(dest => dest.DueDate, 
            opt => 
                opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.BoardId, 
            opt => 
                opt.MapFrom(src => src.BoardId))
            .ForMember(dest => dest.Leader, 
            opt => 
                opt.Ignore())
            .ForMember(dest => dest.Author, 
            opt => 
                opt.Ignore())
            .ForMember(dest => dest.Status, 
            opt => 
                opt.Ignore())
            .ForMember(dest => dest.Id, 
            opt => 
                opt.Ignore())
            .ForMember(dest => dest.TaskWorkers, 
            opt => 
                opt.Ignore());

        CreateMap<UpdateTaskRequest, TaskDto>()
            .ForMember(dest => dest.Id, 
            opt => 
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BoardId, 
            opt => 
                opt.MapFrom(src => src.BoardId))
            .ForMember(dest => dest.Title, 
            opt => 
                opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.Title, 
            opt => 
                opt.MapFrom(src => src.Title!))
            .ForMember(dest => dest.Description, 
            opt => 
                opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.Description, 
            opt => 
                opt.MapFrom(src => src.Description!))
            .ForMember(dest => dest.PriorityLevel, 
            opt => 
                opt.Condition(src => src.PriorityLevel.HasValue))
            .ForMember(dest => dest.PriorityLevel, 
            opt => 
                opt.MapFrom(src => src.PriorityLevel!.Value))
            .ForMember(dest => dest.DueDate, 
            opt => 
                opt.Condition(src => src.DueDate.HasValue))
            .ForMember(dest => dest.DueDate, 
            opt => 
                opt.MapFrom(src => src.DueDate!.Value));

    }
}