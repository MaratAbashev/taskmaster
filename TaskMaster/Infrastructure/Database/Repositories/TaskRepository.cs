using AutoMapper;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Domain.Models.TaskStatus;

namespace Infrastructure.Database.Repositories;

public class TaskRepository(AppDbContext context, IMapper mapper) : Repository<ToDoTask, long>(context), ITaskRepository
{
    public async Task<Result<UserDto>> FollowOnTask(long taskId, long userId)
    {
        try
        {
            var task = await GetByFilterAsync(t => t.Id == taskId);
            if (task == null)
                return Result<UserDto>.Failure("Task not found", ErrorType.NotFound);
            var boardUser = await context.BoardUsers
                .Include(bu => bu.User)
                .FirstOrDefaultAsync(bu => bu.BoardId == task.BoardId && bu.UserId == userId);
            if (boardUser == null)
                return Result<UserDto>.Failure("Board user not found", ErrorType.NotFound);
            if (task.Status > TaskStatus.InProgress)
                return Result<UserDto>.Failure($"Can't follow on {task.Status} task", ErrorType.BadRequest);
            var existingWorker = await context.TaskWorkers
                .FirstOrDefaultAsync(tw => tw.TaskId == taskId && tw.UserId == userId);
            if (existingWorker != null)
                return Result<UserDto>.Failure("Task worker already exists", ErrorType.BadRequest);
            var taskWorker = new TaskWorker
            {
                User = boardUser.User,
                Task = task,
                IsConfirmed = false,
                StartedAt = null,
                FinishedAt = null
            };
            await context.TaskWorkers.AddAsync(taskWorker);
            await context.SaveChangesAsync();
            return Result<UserDto>.Success(mapper.Map<UserDto>(taskWorker));
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<UserDto>> UnfollowOnTask(long taskId, long userId)
    {
        try
        {
            var task = await GetByFilterAsync(t => t.Id == taskId);
            if (task == null)
                return Result<UserDto>.Failure("Task not found", ErrorType.NotFound);
            var taskWorker = await context.TaskWorkers
                .Include(bu => bu.User)
                .FirstOrDefaultAsync(bu => bu.TaskId == taskId && bu.UserId == userId);
            if (task.Status > TaskStatus.InProgress)
                return Result<UserDto>.Failure($"Can't follow on {task.Status} task", ErrorType.BadRequest);
            switch (taskWorker)
            {
                case null:
                    return Result<UserDto>.Failure("Task worker not found", ErrorType.NotFound);
                case { IsConfirmed: true, StartedAt: not null }:
                    taskWorker.User.SocialRating -= 10;
                    break;
            }
            var userDto = mapper.Map<UserDto>(taskWorker);
            context.TaskWorkers.Remove(taskWorker);
            await context.SaveChangesAsync();
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<TaskDto>> CreateTask(TaskDto taskDto, long creatorId)
    {
        try
        {
            var author = await context.BoardUsers
                .Include(bu => bu.User)
                .FirstOrDefaultAsync(bu => bu.BoardId == taskDto.BoardId 
                                           && bu.UserId == creatorId);
            var leader = await context.BoardUsers
                .Include(bu => bu.User)
                .FirstOrDefaultAsync(bu => bu.BoardId == taskDto.BoardId
                                           && bu.UserId == taskDto.LeaderId);
            if (author == null)
                return Result<TaskDto>.Failure("Author not found", ErrorType.NotFound);
            if (leader == null)
                return Result<TaskDto>.Failure("Leader not found", ErrorType.NotFound);
            if (!author.CanCreateTasks)
                return Result<TaskDto>.Failure("Can't create task", ErrorType.Forbidden);
            var task = mapper.Map<ToDoTask>(taskDto);
            task.CreationDate = DateTime.UtcNow;
            task.Status = TaskStatus.ToDo;
            task.AuthorId = author.UserId;
            task.LeaderId = leader.UserId;
            task.Author = author.User;
            task.Leader = leader.User;
            return Result<TaskDto>.Success(
                mapper.Map<TaskDto>(await AddAsync(task)));
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<TaskDto>> UpdateTask(TaskDto taskDto, long updaterId)
    {
        try
        {
            var author = await context.BoardUsers
                .FirstOrDefaultAsync(bu => bu.BoardId == taskDto.BoardId
                                           && bu.UserId == updaterId);
            if (author == null)
                return Result<TaskDto>.Failure("Author not found", ErrorType.NotFound);
            var updatedTask = await PatchAsync(taskDto.Id, t =>
            {
                if (t.AuthorId != author.UserId
                    && !author.IsOwner) throw new UnauthorizedAccessException("This user can't update task");
                t.Description = taskDto.Description;
                t.Title = taskDto.Title ?? t.Title;
                t.PriorityLevel = taskDto.PriorityLevel ?? t.PriorityLevel;
                t.DueDate = taskDto.DueDate > t.CreationDate
                    ? taskDto.DueDate ?? t.DueDate
                    : throw new ArgumentException("New Deadline is invalid");
            });
            return Result<TaskDto>.Success(
                mapper.Map<TaskDto>(updatedTask));
        }
        catch (KeyNotFoundException)
        {
            return Result<TaskDto>.Failure("Task not found", ErrorType.NotFound);
        }
        catch (ArgumentException ex)
        {
            return Result<TaskDto>.Failure(ex.Message, ErrorType.BadRequest);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Result<TaskDto>.Failure(ex.Message, ErrorType.Forbidden);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteTask(long taskId, long userId)
    {
        try
        {
            var task = await GetByFilterWithoutTrackingAsync(t => t.Id == taskId);
            if (task == null)
                return Result.Failure("Task not found", ErrorType.NotFound);
            var author = await context.BoardUsers
                .FirstOrDefaultAsync(bu => bu.BoardId == task.BoardId 
                                           && bu.UserId == userId);
            if (author == null)
                return Result.Failure("Author not found", ErrorType.NotFound);
            if (task.AuthorId != author.UserId
                && !author.IsOwner) return Result.Failure("Can't delete this task", ErrorType.Forbidden);
            await DeleteAsync(t => t.Id == taskId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<TaskDto>> SendTaskToApprove(long taskId, long userId)
    {
        try
        {
            var task = await PatchAsync(taskId, t =>
            {
                if (t.LeaderId != userId) 
                    throw new UnauthorizedAccessException("This user can't send task to approve");
                if (t.Status >= TaskStatus.OnReview)
                    throw new ArgumentException("Task can not be sent to review");
                t.Status = TaskStatus.OnReview;
                t.SentToApproveDate = DateTime.UtcNow;
            });
            return Result<TaskDto>.Success(
                mapper.Map<TaskDto>(task));
        }
        catch (KeyNotFoundException)
        {
            return Result<TaskDto>.Failure("Task not found", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<TaskDto>> ApproveTask(long taskId, long userId)
    {
        try
        {
            var task = await _dbSet
                .Include(t => t.TaskWorkers)!
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return Result<TaskDto>.Failure("Task not found", ErrorType.NotFound);
            var author = await context.BoardUsers
                .FirstOrDefaultAsync(bu => bu.BoardId == task.BoardId
                && bu.UserId == userId);
            if (author == null)
                return Result<TaskDto>.Failure("Author not found", ErrorType.NotFound);
            if (task.AuthorId != userId
                && !author.IsOwner) return Result<TaskDto>.Failure("Can't delete this task", ErrorType.Forbidden);
            if (task.Status != TaskStatus.OnReview)
                return Result<TaskDto>.Failure("Task can not be approved", ErrorType.BadRequest);
            task.Status = TaskStatus.Approved;
            task.ApprovingDate = DateTime.UtcNow;
            var workers = task.TaskWorkers?
                .Where(tw => tw.IsConfirmed)
                .ToList();
            workers?.ForEach(tw =>
            {
                if (task.ApprovingDate > task.DueDate)
                {
                    switch (tw)
                    {
                        case { StartedAt: null }:
                            tw.User.SocialRating -= 20;
                            break;
                        case { StartedAt: not null, FinishedAt: null }:
                        case { StartedAt: not null } when tw.FinishedAt > task.DueDate:
                            tw.User.SocialRating -= 10;
                            break;
                    }
                }
                else
                {
                    switch (tw)
                    {
                        case { StartedAt: null }:
                            break;
                        case { StartedAt: not null, FinishedAt: null }:
                            tw.User.SocialRating += 5;
                            break;
                        case { StartedAt: not null, FinishedAt: not null }:
                            tw.User.SocialRating += 20;
                            break;
                    }
                }
            });
            await context.SaveChangesAsync();
            return Result<TaskDto>.Success(mapper.Map<TaskDto>(task));
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }
    
    public async Task<Result<TaskDto>> DeclineTask(long taskId, long userId, TaskStatus status)
    {
        try
        {
            var task = await _dbSet
                .Include(t => t.TaskWorkers)!
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return Result<TaskDto>.Failure("Task not found", ErrorType.NotFound);
            var author = await context.BoardUsers
                .FirstOrDefaultAsync(bu => bu.BoardId == task.BoardId
                && bu.UserId == userId);
            if (author == null)
                return Result<TaskDto>.Failure("Author not found", ErrorType.NotFound);
            if (task.AuthorId != userId
                && !author.IsOwner) return Result<TaskDto>.Failure("Can't delete this task", ErrorType.Forbidden);
            if (task.Status != TaskStatus.OnReview)
                return Result<TaskDto>.Failure("Task can not be declined", ErrorType.BadRequest);
            task.Status = status;
            if (status == TaskStatus.InProgress)
            {
                await context.SaveChangesAsync();
                return Result<TaskDto>.Success(mapper.Map<TaskDto>(task));
            }
            task.ApprovingDate = DateTime.UtcNow;
            var workers = task.TaskWorkers?
                .Where(tw => tw.IsConfirmed)
                .ToList();
            workers?.ForEach(tw =>
            {
                if (task.ApprovingDate > task.DueDate)
                {
                    switch (tw)
                    {
                        case { StartedAt: null }:
                            tw.User.SocialRating -= 30;
                            break;
                        case { StartedAt: not null, FinishedAt: null }:
                        case { StartedAt: not null } when tw.FinishedAt > task.DueDate:
                            tw.User.SocialRating -= 20;
                            break;
                        case { StartedAt: not null } when tw.FinishedAt <= task.DueDate:
                            tw.User.SocialRating -= 10;
                            break;
                    }
                }
                else
                {
                    switch (tw)
                    {
                        case { StartedAt: null }:
                            break;
                        case { StartedAt: not null, FinishedAt: null }:
                            tw.User.SocialRating -= 5;
                            break;
                        case { StartedAt: not null, FinishedAt: not null }:
                            tw.User.SocialRating -= 10;
                            break;
                    }
                }
            });
            await context.SaveChangesAsync();
            return Result<TaskDto>.Success(mapper.Map<TaskDto>(task));
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }
}