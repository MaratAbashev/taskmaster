namespace Domain.Models;

/// <summary>
/// DTO для токенов аутентификации.
/// </summary>
public class TokenDto
{
    /// <summary>
    /// Refresh-токен.
    /// </summary>
    public string? RefreshToken { get; set; }
    /// <summary>
    /// Access-токен.
    /// </summary>
    public string? AccessToken { get; set; }
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long? UserId { get; set; }
    /// <summary>
    /// Флаг, указывающий был ли токен использован.
    /// </summary>
    public bool? IsUsed { get; set; }
    /// <summary>
    /// Дата создания refresh-токена.
    /// </summary>
    public DateTime? RefreshCreatedAt { get; set; }
    /// <summary>
    /// Дата истечения срока действия refresh-токена.
    /// </summary>
    public DateTime? RefreshExpiresAt { get; set; }
}