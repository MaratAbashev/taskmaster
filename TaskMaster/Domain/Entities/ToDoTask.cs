using Domain.Abstractions;
using Domain.Models;
using TaskStatus = Domain.Models.TaskStatus;

namespace Domain.Entities;

/// <summary>
/// Представляет задачу в системе.
/// </summary>
public class ToDoTask : Entity<long>
{
    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Текущий статус задачи.
    /// </summary>
    public TaskStatus Status { get; set; }
    /// <summary>
    /// Уровень приоритета задачи.
    /// </summary>
    public PriorityLevel PriorityLevel { get; set; }
    /// <summary>
    /// Дата создания задачи.
    /// </summary>
    public DateTime CreationDate { get; set; }
    /// <summary>
    /// Срок выполнения задачи.
    /// </summary>
    public DateTime DueDate { get; set; }
    /// <summary>
    /// Дата одобрения задачи.
    /// </summary>
    public DateTime? ApprovingDate { get; set; }
    /// <summary>
    /// Дата отправки задачи на проверку.
    /// </summary>
    public DateTime? SentToApproveDate { get; set; }
    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid BoardId { get; set; }
    /// <summary>
    /// Связанная доска задач.
    /// </summary>
    public TaskBoard Board { get; set; } = null!;
    /// <summary>
    /// Идентификатор руководителя задачи.
    /// </summary>
    public long? LeaderId { get; set; }
    /// <summary>
    /// Идентификатор автора задачи.
    /// </summary>
    public long? AuthorId { get; set; }
    /// <summary>
    /// Автор задачи.
    /// </summary>
    public User? Author { get; set; }
    /// <summary>
    /// Руководитель задачи.
    /// </summary>
    public User? Leader { get; set; }

    /// <summary>
    /// Список исполнителей задачи.
    /// </summary>
    public List<TaskWorker>? TaskWorkers { get; set; } = [];
}

