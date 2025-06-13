namespace Application.Models;

/// <summary>
/// Модель ответа с информацией о пользователе доски задач.
/// </summary>
public class TaskBoardUserResponse
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Отображаемое имя пользователя.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// URL фотографии пользователя.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Флаг, указывающий является ли пользователь владельцем доски.
    /// </summary>
    public bool IsOwner { get; set; }

    /// <summary>
    /// Флаг, указывающий может ли пользователь создавать задачи.
    /// </summary>
    public bool CanCreateTasks { get; set; }

    /// <summary>
    /// Социальный рейтинг пользователя.
    /// </summary>
    public int? SocialRating { get; set; }
}