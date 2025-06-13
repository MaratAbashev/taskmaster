using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Repositories;

/// <summary>
/// Репозиторий для работы с исполнителями задач.
/// </summary>
public interface ITaskWorkerRepository
{
    /// <summary>
    /// Начинает работу над задачей.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> StartWorkingOnTask(long taskId, long userId);
    /// <summary>
    /// Завершает работу над задачей.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> FinishTask(long taskId, long userId);
    /// <summary>
    /// Подтверждает пользователя как исполнителя задачи.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="confirmerId">Идентификатор подтверждающего.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> ConfirmUser(long taskId, long userId, long confirmerId);
}