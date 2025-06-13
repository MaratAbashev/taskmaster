using AutoMapper;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class TaskBoardRepository(AppDbContext context, IMapper mapper) : Repository<TaskBoard, Guid>(context), ITaskBoardRepository
{
    public async Task<Result<TaskBoardDto>> GetTaskBoardByTelegramGroupId(long telegramGroupId)
    {
        try
        {
            var board = await _dbSet
                .Include(tb => tb.BoardTasks)
                .ThenInclude(t => t.TaskWorkers)!
                .ThenInclude(t => t.User)
                .Include(tb => tb.BoardUsers)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(b => b.GroupId == telegramGroupId);
            return board == null 
                ? Result<TaskBoardDto>.Failure("TaskBoard not found", ErrorType.NotFound) 
                : Result<TaskBoardDto>.Success(mapper.Map<TaskBoardDto>(board));
        }
        catch (Exception ex)
        {
            return Result<TaskBoardDto>.Failure(ex.Message);
        }
    }
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

    public async Task<Result<TaskBoardDto>> GetTaskBoardById(Guid boardId, long userId)
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
            if (board == null)
                return Result<TaskBoardDto>.Failure("Board not found", ErrorType.NotFound);
            if (board.BoardUsers.Any(bu => bu.UserId == userId))
                return Result<TaskBoardDto>.Success(mapper.Map<TaskBoardDto>(board));
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Result<TaskBoardDto>.Failure("User not found", ErrorType.NotFound);
            board.BoardUsers.Add(new BoardUser
            {
                IsOwner = false,
                CanCreateTasks = false,
                UserId = userId,
                BoardId = board.Id,
                User = user
            });
            await context.SaveChangesAsync();
            return Result<TaskBoardDto>.Success(mapper.Map<TaskBoardDto>(board));
        }
        catch (Exception ex)
        {
            return Result<TaskBoardDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteTaskBoard(Guid boardId, long userId)
    {
        try
        {
            var boardUser = await context.BoardUsers.Where(bu => bu.BoardId == boardId && bu.UserId == userId).SingleOrDefaultAsync();
            if (boardUser == null)
                return Result.Failure("User not found", ErrorType.NotFound);
            if (!boardUser.IsOwner)
                return Result.Failure("User doesn't have permission to delete this", ErrorType.Forbidden);
            await DeleteAsync(tb => tb.Id == boardId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<TaskBoardDto>> RenameTaskBoard(Guid boardId, long userId, string newTitle)
    {
        try
        {
            var boardUser = await context.BoardUsers.Where(bu => bu.BoardId == boardId && bu.UserId == userId).SingleOrDefaultAsync();
            if (boardUser == null)
                return Result<TaskBoardDto>.Failure("User not found", ErrorType.NotFound);
            if (!boardUser.IsOwner)
                return Result<TaskBoardDto>.Failure("User doesn't have permission to rename this", ErrorType.Forbidden);
            var board = await _dbSet
                .Include(b => b.BoardUsers)
                .ThenInclude(bu => bu.User)
                .Include(b => b.BoardTasks)
                .FirstOrDefaultAsync(b => b.Id == boardId);
            if (board == null)
                return Result<TaskBoardDto>.Failure("Board not found", ErrorType.NotFound);
            board.Title = newTitle;
            await context.SaveChangesAsync();
            return Result<TaskBoardDto>.Success(mapper.Map<TaskBoardDto>(board));
        }
        catch (KeyNotFoundException)
        {
            return Result<TaskBoardDto>.Failure("Board not found", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            return Result<TaskBoardDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<List<TaskBoardDto>>> GetAllTaskBoardsByUserId(long userId)
    {
        try
        {
            var taskBoards = await context.BoardUsers
                .Where(b => b.UserId == userId)
                .Include(bu => bu.Board)
                .ThenInclude(b => b.BoardUsers)
                .ThenInclude(bu => bu.User)
                .Select(bu => bu.Board)
                .ToListAsync();
            return Result<List<TaskBoardDto>>.Success(mapper.Map<List<TaskBoardDto>>(taskBoards));
        }
        catch (Exception ex)
        {
            return Result<List<TaskBoardDto>>.Failure(ex.Message);
        }
    }
}