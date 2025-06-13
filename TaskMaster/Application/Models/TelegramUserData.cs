namespace Application.Models;

/// <summary>
/// Модель данных пользователя Telegram.
/// </summary>
public class TelegramUserData
{
    /// <summary>
    /// Идентификатор пользователя в Telegram.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Имя пользователя в Telegram.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// URL фотографии пользователя.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Дата аутентификации пользователя.
    /// </summary>
    public long AuthDate { get; set; }

    /// <summary>
    /// Хеш данных пользователя для проверки подлинности.
    /// </summary>
    public string? Hash { get; set; }
}