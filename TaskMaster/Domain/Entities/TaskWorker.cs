using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Представляет связь между пользователем и задачей, над которой он работает.
/// </summary>
public class TaskWorker: Entity<long>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// Связанный пользователь.
    /// </summary>
    public User User { get; set; } = null!;
    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public long TaskId { get; set; }
    /// <summary>
    /// Связанная задача.
    /// </summary>
    public ToDoTask Task { get; set; } = null!;

    /// <summary>
    /// Дата начала работы над задачей.
    /// </summary>
    public DateTime? StartedAt { get; set; }
    /// <summary>
    /// Дата завершения работы над задачей.
    /// </summary>
    public DateTime? FinishedAt { get; set; }
    /// <summary>
    /// Флаг, указывающий подтвержден ли пользователь как исполнитель задачи.
    /// </summary>
    public bool IsConfirmed { get; set; }
}
