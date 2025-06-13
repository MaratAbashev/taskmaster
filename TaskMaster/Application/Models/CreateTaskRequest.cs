using Domain.Models;

namespace Application.Models;

/// <summary>
/// Модель запроса на создание задачи.
/// </summary>
public class CreateTaskRequest
{
    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid BoardId { get; set; }

    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Уровень приоритета задачи.
    /// </summary>
    public PriorityLevel PriorityLevel { get; set; }

    /// <summary>
    /// Срок выполнения задачи.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Идентификатор руководителя задачи.
    /// </summary>
    public long LeaderId { get; set; }
}
