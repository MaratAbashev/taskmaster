using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Представляет Telegram-группу, связанную с доской задач.
/// </summary>
public class TelegramGroup: Entity<long>
{
    /// <summary>
    /// Ссылка на Telegram-группу.
    /// </summary>
    public string? Link { get; set; }
    /// <summary>
    /// Связанная доска задач.
    /// </summary>
    public TaskBoard? Board { get; set; }
    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid BoardId { get; set; }
}