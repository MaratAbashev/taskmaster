using Domain.Models;

namespace Application.Models;

/// <summary>
/// Модель запроса на обновление задачи.
/// </summary>
public class UpdateTaskRequest
{
    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid BoardId { get; set; }

    /// <summary>
    /// Новое название задачи.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Новое описание задачи.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Новый уровень приоритета задачи.
    /// </summary>
    public PriorityLevel? PriorityLevel { get; set; }

    /// <summary>
    /// Новый срок выполнения задачи.
    /// </summary>
    public DateTime? DueDate { get; set; }
}