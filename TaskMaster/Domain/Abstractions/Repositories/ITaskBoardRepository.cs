using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Repositories;

/// <summary>
/// Репозиторий для работы с досками задач.
/// </summary>
public interface ITaskBoardRepository
{
    Task<Result<TaskBoardDto>> GetTaskBoardByTelegramGroupId(long telegramGroupId);
    /// <summary>
    /// Создает новую доску задач.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="title">Название доски.</param>
    /// <returns>Результат с данными созданной доски.</returns>
    Task<Result<TaskBoardDto>> CreateTaskBoard(long userId, string title);
    /// <summary>
    /// Получает доску задач по идентификатору.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными доски.</returns>
    Task<Result<TaskBoardDto>> GetTaskBoardById(Guid boardId, long userId);
    /// <summary>
    /// Удаляет доску задач.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат выполнения операции.</returns>
    Task<Result> DeleteTaskBoard(Guid boardId, long userId);
    /// <summary>
    /// Переименовывает доску задач.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="newTitle">Новое название доски.</param>
    /// <returns>Результат с данными обновленной доски.</returns>
    Task<Result<TaskBoardDto>> RenameTaskBoard(Guid boardId, long userId, string newTitle);
    /// <summary>
    /// Получает все доски задач пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат со списком досок.</returns>
    Task<Result<List<TaskBoardDto>>> GetAllTaskBoardsByUserId(long userId);
}