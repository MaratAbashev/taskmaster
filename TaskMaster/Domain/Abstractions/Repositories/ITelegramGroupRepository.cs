using Domain.Utils;

namespace Domain.Abstractions.Repositories;

public interface ITelegramGroupRepository
{
    Task<Result> LinkGroupToBoard(long groupId, Guid boardId);
    Task<Result<long>> GetGroupByBoardId(Guid boardId);
    Task<Result<long>> GetGroupByTaskId(long taskId);
}