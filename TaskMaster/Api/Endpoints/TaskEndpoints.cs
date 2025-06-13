using System.Security.Claims;
using Api.Extensions;
using Application.Models;
using AutoMapper;
using Domain.Abstractions.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = Domain.Models.TaskStatus;

namespace Api.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var taskGroup = endpoints.MapGroup("/tasks").WithTags("Task");

        taskGroup.MapPost("/create", async (
                [FromBody] CreateTaskRequest createTaskRequest,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var createResult = await taskService.CreateTask(mapper.Map<TaskDto>(createTaskRequest), userId);
            return createResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskResponse>(createResult.Data))
                : createResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskResponse>();

        taskGroup.MapPut("/update", async (
                [FromBody] UpdateTaskRequest updateTaskRequest,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var updateResult = await taskService.UpdateTask(mapper.Map<TaskDto>(updateTaskRequest), userId);
            return updateResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskResponse>(updateResult.Data))
                : updateResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskResponse>();

        taskGroup.MapDelete("/{taskId:long}", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var deleteResult = await taskService.DeleteTask(taskId, userId);
            return deleteResult.IsSuccess
                ? Results.NoContent()
                : deleteResult.ToErrorHttpResult();
        })
        .RequireAuthorization();

        taskGroup.MapPost("/{taskId:long}/follow", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var followResult = await taskService.FollowOnTask(taskId, userId);
            return followResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskUserResponse>(followResult.Data))
                : followResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskUserResponse>();

        taskGroup.MapPost("/{taskId:long}/unfollow", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var unfollowResult = await taskService.UnfollowOnTask(taskId, userId);
            return unfollowResult.IsSuccess
                ? Results.Ok(mapper.Map<TaskUserResponse>(unfollowResult.Data))
                : unfollowResult.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskUserResponse>();

        taskGroup.MapPost("/{taskId:long}/send-to-approve", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var result = await taskService.SendTaskToApprove(taskId, userId);
            return result.IsSuccess
                ? Results.Ok(mapper.Map<TaskResponse>(result.Data))
                : result.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskResponse>();

        taskGroup.MapPost("/{taskId:long}/approve", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var result = await taskService.ApproveTask(taskId, userId);
            return result.IsSuccess
                ? Results.Ok(mapper.Map<TaskResponse>(result.Data))
                : result.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskResponse>();

        taskGroup.MapPost("/{taskId:long}/decline", async (
                long taskId,
                [FromQuery] string taskStatus,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();
            var status = Enum.Parse<TaskStatus>(taskStatus);
            var result = await taskService.DeclineTask(taskId, userId, status);
            return result.IsSuccess
                ? Results.Ok(mapper.Map<TaskResponse>(result.Data))
                : result.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskResponse>();

        taskGroup.MapPost("/{taskId:long}/start", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var result = await taskService.StartWorkingOnTask(taskId, userId);
            return result.IsSuccess
                ? Results.Ok(mapper.Map<TaskUserResponse>(result.Data))
                : result.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskUserResponse>();

        taskGroup.MapPost("/{taskId:long}/finish", async (
                long taskId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var userIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var result = await taskService.FinishTask(taskId, userId);
            return result.IsSuccess
                ? Results.Ok(mapper.Map<TaskUserResponse>(result.Data))
                : result.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskUserResponse>();

        taskGroup.MapPost("/{taskId:long}/confirm/{userId:long}", async (
                long taskId,
                long userId,
                ClaimsPrincipal user,
                ITaskService taskService,
                IMapper mapper) =>
        {
            var confirmerIdString = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!long.TryParse(confirmerIdString, out var confirmerId))
                return Results.Unauthorized();

            var result = await taskService.ConfirmUser(taskId, userId, confirmerId);
            return result.IsSuccess
                ? Results.Ok(mapper.Map<TaskUserResponse>(result.Data))
                : result.ToErrorHttpResult();
        })
        .RequireAuthorization()
        .Produces<TaskUserResponse>();

        return endpoints;
    }
}