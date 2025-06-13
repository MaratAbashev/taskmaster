namespace Application.Models;

/// <summary>
/// Модель ответа с информацией о доске задач.
/// </summary>
public class TaskBoardResponse
{
    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название доски задач.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Ссылка на Telegram-группу доски задач.
    /// </summary>
    public string? TelegramGroupLink { get; set; }

    /// <summary>
    /// Список пользователей, имеющих доступ к доске задач.
    /// </summary>
    public List<TaskBoardUserResponse>? Users { get; set; }

    /// <summary>
    /// Список задач на доске.
    /// </summary>
    public List<TaskResponse>? Tasks { get; set; }
}
