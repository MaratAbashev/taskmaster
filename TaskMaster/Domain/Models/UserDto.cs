namespace Domain.Models;

/// <summary>
/// DTO для пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Идентификатор пользователя.
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
    /// Социальный рейтинг пользователя.
    /// </summary>
    public int? SocialRating { get; set; }
    /// <summary>
    /// Имя пользователя для упоминания в Telegram.
    /// </summary>
    public string NameToPing
    {
        get
        {
            var nameToPing = string.IsNullOrEmpty(FirstName) 
                ? "Пользователь"
                : string.IsNullOrEmpty(LastName) 
                    ? FirstName 
                    : $"{FirstName} {LastName}";
            return !string.IsNullOrEmpty(Username)
                ? $"@{Username}"
                : $"[{nameToPing}](tg://user?id={Id})";
        }
    }
    /// <summary>
    /// Дата аутентификации пользователя.
    /// </summary>
    public long? AuthDate { get; set; }
    /// <summary>
    /// Хеш данных пользователя для проверки подлинности.
    /// </summary>
    public string? Hash { get; set; }
    /// <summary>
    /// Флаг, указывающий является ли пользователь владельцем доски.
    /// </summary>
    public bool? IsOwner { get; set; }
    /// <summary>
    /// Флаг, указывающий может ли пользователь создавать задачи на доске.
    /// </summary>
    public bool? CanCreateTasks { get; set; }
    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid? BoardId { get; set; }
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
    public bool? IsConfirmed { get; set; }
}