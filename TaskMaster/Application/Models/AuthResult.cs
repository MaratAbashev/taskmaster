namespace Application.Models;

/// <summary>
/// Модель результата аутентификации.
/// </summary>
public class AuthResult
{
    /// <summary>
    /// JWT токен доступа.
    /// </summary>
    public string Token { get; set; }
}