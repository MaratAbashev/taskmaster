using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Services;

/// <summary>
/// Сервис для работы с досками задач.
/// </summary>
public interface ITaskBoardService
{
    /// <summary>
    /// Создает новую доску задач.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="title">Название доски.</param>
    /// <returns>Результат с данными созданной доски.</returns>
    Task<Result<TaskBoardDto>> CreateTaskBoard(long userId, string title);
    /// <summary>
    /// Обновляет название доски задач.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="newTitle">Новое название доски.</param>
    /// <returns>Результат с данными обновленной доски.</returns>
    Task<Result<TaskBoardDto>> UpdateTaskBoard(Guid boardId, long userId, string newTitle);
    /// <summary>
    /// Удаляет доску задач.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат выполнения операции.</returns>
    Task<Result> DeleteTaskBoard(Guid boardId, long userId);
    /// <summary>
    /// Получает доску задач по идентификатору.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными доски.</returns>
    Task<Result<TaskBoardDto>> GetTaskBoard(Guid boardId, long userId);
    /// <summary>
    /// Получает все доски задач пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат со списком досок.</returns>
    Task<Result<List<TaskBoardDto>>> GetUserTaskBoards(long userId);
}