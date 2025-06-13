using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Services;

/// <summary>
/// Сервис для работы с JWT токенами.
/// </summary>
public interface IJwtWorkerService
{
    /// <summary>
    /// Создает пару токенов (access и refresh) для пользователя.
    /// </summary>
    /// <param name="userDto">Данные пользователя.</param>
    /// <returns>Результат с токенами.</returns>
    Result<TokenDto> CreateTokens(UserDto userDto);
}