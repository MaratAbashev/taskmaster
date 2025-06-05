using Domain.Models;
using Domain.Utils;

namespace Infrastructure.Database.Repositories;

public interface ITaskBoardRepository
{
    Task<Result<TaskBoardDto>> CreateTaskBoard(long userId, string title);
    Task<Result<TaskBoardDto>> GetTaskBoardById(Guid boardId);
    Task<Result> DeleteTaskBoard(Guid boardId);
}