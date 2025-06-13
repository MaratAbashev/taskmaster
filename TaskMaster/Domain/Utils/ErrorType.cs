namespace Domain.Utils;

/// <summary>
/// Типы ошибок, которые могут возникнуть в системе.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Общая ошибка.
    /// </summary>
    Generic,
    /// <summary>
    /// Ошибка аутентификации.
    /// </summary>
    Unauthorized,
    /// <summary>
    /// Ресурс не найден.
    /// </summary>
    NotFound,
    /// <summary>
    /// Ошибка валидации данных.
    /// </summary>
    Validation,
    /// <summary>
    /// Конфликт данных.
    /// </summary>
    Conflict,
    /// <summary>
    /// Некорректный запрос.
    /// </summary>
    BadRequest,
    /// <summary>
    /// Отказано в доступе.
    /// </summary>
    Forbidden,
    /// <summary>
    /// Необработанное исключение.
    /// </summary>
    Exception
}