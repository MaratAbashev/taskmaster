using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class TaskBoardRepository(AppDbContext context, IMapper mapper) : Repository<TaskBoard, Guid>(context), ITaskBoardRepository
{
    public async Task<Result<TaskBoardDto>> CreateTaskBoard(long userId, string title)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Result<TaskBoardDto>.Failure("User not found", ErrorType.NotFound);
            var boardUser = new BoardUser
            {
                User = user,
                UserId = userId,
                IsOwner = true,
                CanCreateTasks = true
            };
            await context.BoardUsers.AddAsync(boardUser);
            var taskBoard = new TaskBoard
            {
                Id = Guid.NewGuid(),
                Title = title,
                BoardUsers = [boardUser]
            };
            await AddAsync(taskBoard);
            return Result<TaskBoardDto>.Success(mapper.Map<TaskBoardDto>(taskBoard));
        }
        catch (Exception ex)
        {
            return Result<TaskBoardDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<TaskBoardDto>> GetTaskBoardById(Guid boardId)
    {
        try
        {
            var board = await _dbSet
                .Where(b => b.Id == boardId)
                .Include(b => b.BoardUsers)
                .ThenInclude(b => b.User)
                .Include(b => b.BoardTasks)
                .ThenInclude(bt => bt.TaskWorkers)
                .Include(b => b.Group)
                .FirstOrDefaultAsync();
            return board == null 
                ? Result<TaskBoardDto>.Failure("Board not found", ErrorType.NotFound) 
                : Result<TaskBoardDto>.Success(mapper.Map<TaskBoardDto>(board));
        }
        catch (Exception ex)
        {
            return Result<TaskBoardDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteTaskBoard(Guid boardId)
    {
        try
        {
            await DeleteAsync(tb => tb.Id == boardId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}