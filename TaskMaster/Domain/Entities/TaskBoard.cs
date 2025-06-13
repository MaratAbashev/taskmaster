using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Представляет доску задач.
/// </summary>
public class TaskBoard : Entity<Guid>
{
    /// <summary>
    /// Название доски задач.
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// Список пользователей, имеющих доступ к доске.
    /// </summary>
    public List<BoardUser> BoardUsers { get; set; } = [];
    /// <summary>
    /// Список задач на доске.
    /// </summary>
    public List<ToDoTask> BoardTasks { get; set; } = [];

    /// <summary>
    /// Связанная Telegram-группа.
    /// </summary>
    public TelegramGroup? Group { get; set; }
    /// <summary>
    /// Идентификатор Telegram-группы.
    /// </summary>
    public long? GroupId { get; set; }
    /// <summary>
    /// Флаг, указывающий есть ли у доски связанная Telegram-группа.
    /// </summary>
    public bool HasTelegramGroup { get; set; }
}
