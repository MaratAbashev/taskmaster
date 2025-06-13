using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Services;

/// <summary>
/// Сервис аутентификации пользователей.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Аутентифицирует пользователя и выдает токены.
    /// </summary>
    /// <param name="user">Данные пользователя.</param>
    /// <returns>Результат с токенами.</returns>
    Task<Result<TokenDto>> AuthenticateUser(UserDto user);
    /// <summary>
    /// Обновляет токены по refresh-токену.
    /// </summary>
    /// <param name="tokenDto">Данные токена.</param>
    /// <returns>Результат с новыми токенами.</returns>
    Task<Result<TokenDto>> RefreshAsync(TokenDto tokenDto);
}