using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Представляет пользователя в системе.
/// </summary>
public class User : Entity<long>
{
    /// <summary>
    /// Имя пользователя в Telegram.
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string? LastName { get; set; }
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
    /// Список refresh-токенов пользователя.
    /// </summary>
    public List<RefreshToken>? RefreshTokens { get; set; }
    /// <summary>
    /// Список досок задач, к которым имеет доступ пользователь.
    /// </summary>
    public List<BoardUser>? BoardUsers { get; set; } = [];
    /// <summary>
    /// Список задач, над которыми работает пользователь.
    /// </summary>
    public List<TaskWorker>? TaskWorkers { get; set; } = [];
    /// <summary>
    /// Список задач, где пользователь является руководителем.
    /// </summary>
    public List<ToDoTask>? LeadershipTasks { get; set; } = [];
    /// <summary>
    /// Список задач, созданных пользователем.
    /// </summary>
    public List<ToDoTask>? AuthoredTasks { get; set; } = [];
}
