using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Repositories;

/// <summary>
/// Репозиторий для работы с refresh-токенами пользователей.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Использует refresh-токен для обновления пользователя.
    /// </summary>
    /// <param name="refreshTokenValue">Значение refresh-токена.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> UseRefreshTokenAsync(string refreshTokenValue);
    /// <summary>
    /// Добавляет новый refresh-токен.
    /// </summary>
    /// <param name="tokenDto">Данные токена.</param>
    /// <returns>Результат выполнения операции.</returns>
    Task<Result> AddRefreshTokenAsync(TokenDto tokenDto);
}