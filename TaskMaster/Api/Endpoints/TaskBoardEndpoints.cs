using System.Security.Claims;
using Api.Extensions;
using Application.Models;
using AutoMapper;
using Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class TaskBoardEndpoints
{
    public static IEndpointRouteBuilder MapTaskBoardEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var taskBoardGroup = endpoints.MapGroup("/taskboards").WithTags("TaskBoard");
        taskBoardGroup.MapGet("/", 
                async (ClaimsPrincipal user, 
                    ITaskBoardService taskBoardService, 
                    IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(cl => cl.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();
            var taskBoardsResult = await taskBoardService.GetUserTaskBoards(userId);
            return taskBoardsResult.IsSuccess
                ? Results.Ok(mapper.Map<List<TaskBoardResponse>>(taskBoardsResult.Data))
                : taskBoardsResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<List<TaskBoardResponse>>();
        taskBoardGroup.MapGet("/{boardId:guid}", 
                async (ClaimsPrincipal user, 
                    Guid boardId, 
                    ITaskBoardService taskBoardService, 
                    IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(cl => cl.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();
            var taskBoardResult = await taskBoardService.GetTaskBoard(boardId, userId);
            return taskBoardResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskBoardResponse>(taskBoardResult.Data))
                : taskBoardResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskBoardResponse>();
        taskBoardGroup.MapPut("/{boardId:guid}/rename", 
                async ([FromQuery] string newTitle, 
                    ClaimsPrincipal user, 
                    Guid boardId, 
                    ITaskBoardService taskBoardService, 
                    IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(cl => cl.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();
            var taskBoardRenameResult = await taskBoardService.UpdateTaskBoard(boardId, userId, newTitle);
            return taskBoardRenameResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskBoardResponse>(taskBoardRenameResult.Data))
                : taskBoardRenameResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskBoardResponse>();
        taskBoardGroup.MapDelete("/{boardId:guid}", 
                async (ClaimsPrincipal user, 
                    Guid boardId, 
                    ITaskBoardService taskBoardService, 
                    IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(cl => cl.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();
            var deleteTaskBoardResult = await taskBoardService.DeleteTaskBoard(boardId, userId);
            return deleteTaskBoardResult.IsSuccess
                ? Results.NoContent()
                : deleteTaskBoardResult.ToErrorHttpResult();
        })
        .RequireAuthorization();
        taskBoardGroup.MapPost("/create", 
                async ([FromQuery] string title, 
                    ClaimsPrincipal user, 
                    ITaskBoardService taskBoardService, 
                    IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(cl => cl.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();
            var createTaskBoardResult = await taskBoardService.CreateTaskBoard(userId, title);
            return createTaskBoardResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskBoardResponse>(createTaskBoardResult.Data))
                : createTaskBoardResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskBoardResponse>();
        return endpoints;
    }
}