using AutoMapper;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Domain.Models.TaskStatus;

namespace Infrastructure.Database.Repositories;

public class TaskWorkerRepository(AppDbContext context, IMapper mapper): Repository<TaskWorker, long>(context), ITaskWorkerRepository
{
    public async Task<Result<UserDto>> StartWorkingOnTask(long taskId, long userId)
    {
        try
        {
            var user = await _dbSet
                .Include(tw => tw.User)
                .FirstOrDefaultAsync(tw => tw.UserId == userId 
                                                    && tw.TaskId == taskId);
            if (user == null)
                return Result<UserDto>.Failure("User not found", ErrorType.NotFound);
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return Result<UserDto>.Failure("Task not found", ErrorType.NotFound);
            if (DateTime.UtcNow >= task.DueDate)
                return Result<UserDto>.Failure("Can't start task after deadline", ErrorType.BadRequest);
            if (task.Status > TaskStatus.OnReview)
                return Result<UserDto>.Failure($"Can't start task that is {task.Status}", ErrorType.BadRequest);
            user.StartedAt = DateTime.UtcNow;
            user.FinishedAt = null;
            if (user.IsConfirmed && task.Status == TaskStatus.ToDo)
                task.Status = TaskStatus.InProgress;
            await context.SaveChangesAsync();
            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<UserDto>> FinishTask(long taskId, long userId)
    {
        try
        {
            var user = await _dbSet
                .Include(tw => tw.User)
                .FirstOrDefaultAsync(tw => tw.UserId == userId 
                                           && tw.TaskId == taskId);
            if (user == null)
                return Result<UserDto>.Failure("User not found", ErrorType.NotFound);
            if (user.StartedAt == null)
                return Result<UserDto>.Failure("Can't finish unstarted task", ErrorType.BadRequest);
            user.FinishedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
        catch (Exception e)
        {
            return Result<UserDto>.Failure(e.Message);
        }
    }

    public async Task<Result<UserDto>> ConfirmUser(long taskId, long userId, long confirmerId)
    {
        try
        {
            var user = await _dbSet
                .Include(tw => tw.User)
                .FirstOrDefaultAsync(tw => tw.UserId == userId 
                                           && tw.TaskId == taskId);
            if (user == null)
                return Result<UserDto>.Failure("User not found", ErrorType.NotFound);
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return Result<UserDto>.Failure("Task not found", ErrorType.NotFound);
            if (task.AuthorId != confirmerId)
                return Result<UserDto>.Failure("This confirmer is not the author of the task", ErrorType.Forbidden);
            if (task.Status > TaskStatus.OnReview)
                return Result<UserDto>.Failure($"Can't confirm user on {task.Status} task");
            user.IsConfirmed = true;
            await context.SaveChangesAsync();
            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
        catch (Exception e)
        {
            return Result<UserDto>.Failure(e.Message);
        }
    }
}