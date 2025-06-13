namespace Domain.Models;

/// <summary>
/// DTO для доски задач.
/// </summary>
public class TaskBoardDto
{
    /// <summary>
    /// Идентификатор доски.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название доски.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Список пользователей, имеющих доступ к доске.
    /// </summary>
    public List<UserDto>? Users { get; set; }
    /// <summary>
    /// Список задач на доске.
    /// </summary>
    public List<TaskDto>? Tasks { get; set; }
    /// <summary>
    /// Ссылка на Telegram-группу доски.
    /// </summary>
    public string? TelegramGroupLink { get; set; }
}