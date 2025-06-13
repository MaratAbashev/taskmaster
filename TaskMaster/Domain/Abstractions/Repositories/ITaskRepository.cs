using Domain.Models;
using Domain.Utils;
using TaskStatus = Domain.Models.TaskStatus;

namespace Domain.Abstractions.Repositories;

/// <summary>
/// Репозиторий для работы с задачами.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Подписывает пользователя на задачу.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> FollowOnTask(long taskId, long userId);
    /// <summary>
    /// Отписывает пользователя от задачи.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> UnfollowOnTask(long taskId, long userId);
    /// <summary>
    /// Создает новую задачу.
    /// </summary>
    /// <param name="taskDto">Данные задачи.</param>
    /// <param name="creatorId">Идентификатор создателя.</param>
    /// <returns>Результат с данными созданной задачи.</returns>
    Task<Result<TaskDto>> CreateTask(TaskDto taskDto, long creatorId);
    /// <summary>
    /// Обновляет существующую задачу.
    /// </summary>
    /// <param name="taskDto">Данные задачи.</param>
    /// <param name="updaterId">Идентификатор обновляющего.</param>
    /// <returns>Результат с данными обновленной задачи.</returns>
    Task<Result<TaskDto>> UpdateTask(TaskDto taskDto, long updaterId);
    /// <summary>
    /// Удаляет задачу.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат выполнения операции.</returns>
    Task<Result> DeleteTask(long taskId, long userId);
    /// <summary>
    /// Отправляет задачу на проверку.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными задачи.</returns>
    Task<Result<TaskDto>> SendTaskToApprove(long taskId, long userId);
    /// <summary>
    /// Одобряет задачу.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными задачи.</returns>
    Task<Result<TaskDto>> ApproveTask(long taskId, long userId);
    /// <summary>
    /// Отклоняет задачу.
    /// </summary>
    /// <param name="taskId">Идентификатор задачи.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="status">Статус задачи.</param>
    /// <returns>Результат с данными задачи.</returns>
    Task<Result<TaskDto>> DeclineTask(long taskId, long userId, TaskStatus status);
}