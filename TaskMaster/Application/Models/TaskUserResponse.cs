namespace Application.Models;

/// <summary>
/// Модель ответа с информацией о пользователе задачи.
/// </summary>
public class TaskUserResponse
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Отображаемое имя пользователя.
    /// </summary>
    public string DisplayName { get; set; } = default!;

    /// <summary>
    /// URL фотографии пользователя.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Социальный рейтинг пользователя.
    /// </summary>
    public int? SocialRating { get; set; }

    /// <summary>
    /// Флаг, указывающий подтвержден ли пользователь как исполнитель задачи.
    /// </summary>
    public bool? IsConfirmed { get; set; }

    /// <summary>
    /// Дата начала работы над задачей.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Дата завершения работы над задачей.
    /// </summary>
    public DateTime? FinishedAt { get; set; }
}
