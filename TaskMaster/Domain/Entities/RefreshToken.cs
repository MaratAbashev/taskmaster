using Domain.Abstractions;

namespace Domain.Entities;

/// <summary>
/// Представляет refresh-токен для аутентификации пользователя.
/// </summary>
public class RefreshToken: Entity<long>
{
    /// <summary>
    /// Значение токена.
    /// </summary>
    public string Token { get; set; }
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// Связанный пользователь.
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    /// Флаг, указывающий был ли токен использован.
    /// </summary>
    public bool IsUsed { get; set; }
    /// <summary>
    /// Дата создания токена.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Дата истечения срока действия токена.
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}