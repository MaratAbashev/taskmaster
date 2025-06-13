using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Представляет связь между пользователем и доской задач.
/// </summary>
public class BoardUser: Entity<long>
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
    /// Идентификатор доски задач.
    /// </summary>
    public Guid BoardId { get; set; }
    /// <summary>
    /// Связанная доска задач.
    /// </summary>
    public TaskBoard? Board { get; set; }

    /// <summary>
    /// Флаг, указывающий является ли пользователь владельцем доски.
    /// </summary>
    public bool IsOwner { get; set; }
    /// <summary>
    /// Флаг, указывающий может ли пользователь создавать задачи на доске.
    /// </summary>
    public bool CanCreateTasks { get; set; }
}
