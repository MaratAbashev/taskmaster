using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class TelegramGroupRepository(AppDbContext context): Repository<TelegramGroup, long>(context), ITelegramGroupRepository
{
    public async Task<Result> LinkGroupToBoard(long groupId, Guid boardId)
    {
        try
        {
            var board = await context.TaskBoards
                .FirstOrDefaultAsync(tb => tb.Id == boardId);
            if (board == null)
                return Result.Failure("Такой доски нет", ErrorType.NotFound);
            var group = new TelegramGroup
            {
                Id = groupId,
                Board = board,
                BoardId = boardId
            };
            board.HasTelegramGroup = true;
            board.GroupId = groupId;
            await AddAsync(group);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<long>> GetGroupByBoardId(Guid boardId)
    {
        try
        {
            var group = await GetByFilterWithoutTrackingAsync(g => g.BoardId == boardId);
            return group == null 
                ? Result<long>.Failure("Group not found", ErrorType.NotFound) 
                : Result<long>.Success(group.Id);
        }
        catch (Exception ex)
        {
            return Result<long>.Failure(ex.Message);
        }
    }

    public async Task<Result<long>> GetGroupByTaskId(long taskId)
    {
        try
        {
            var task = await context.Tasks
                .Include(t => t.Board)
                .ThenInclude(b => b.Group)
                .FirstOrDefaultAsync(t => t.Id == taskId);
            return task == null 
                ? Result<long>.Failure("Group not found", ErrorType.NotFound) 
                : task.Board.HasTelegramGroup
                    ? Result<long>.Success(task.Board.Group!.Id)
                    : Result<long>.Failure("Board has no group", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            return Result<long>.Failure(ex.Message);
        }
    }
}