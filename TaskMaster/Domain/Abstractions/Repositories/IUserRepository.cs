using Domain.Models;
using Domain.Utils;

namespace Domain.Abstractions.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Обновляет или добавляет пользователя.
    /// </summary>
    /// <param name="userDto">Данные пользователя.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> UpdateOrAddUser(UserDto userDto);
    /// <summary>
    /// Получает пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат с данными пользователя.</returns>
    Task<Result<UserDto>> GetUserById(long userId);
}