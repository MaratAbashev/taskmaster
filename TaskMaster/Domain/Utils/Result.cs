namespace Domain.Utils;

/// <summary>
/// Обобщенный класс для представления результата операции с данными.
/// </summary>
/// <typeparam name="T">Тип данных результата.</typeparam>
public class Result<T>(bool isSuccess, T data, string? errorMessage, ErrorType errorType = default)
{
    /// <summary>
    /// Флаг успешности операции.
    /// </summary>
    public bool IsSuccess { get; } = isSuccess;
    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string? ErrorMessage { get; } = errorMessage;
    /// <summary>
    /// Данные результата.
    /// </summary>
    public T Data { get; } = data;
    /// <summary>
    /// Тип ошибки.
    /// </summary>
    public ErrorType ErrorType { get; } = errorType;

    /// <summary>
    /// Создает успешный результат с данными.
    /// </summary>
    /// <param name="data">Данные результата.</param>
    /// <returns>Успешный результат.</returns>
    public static Result<T> Success(T data) => new(true, data, null);

    /// <summary>
    /// Создает неуспешный результат с ошибкой.
    /// </summary>
    /// <param name="error">Сообщение об ошибке.</param>
    /// <param name="errorType">Тип ошибки.</param>
    /// <returns>Неуспешный результат.</returns>
    public static Result<T> Failure(string error, ErrorType errorType = ErrorType.Exception) =>
        new(false, default!, error, errorType);
    
    /// <summary>
    /// Создает неуспешный результат с теми же ошибками, но другим типом данных.
    /// </summary>
    /// <typeparam name="TResult">Новый тип данных результата.</typeparam>
    /// <returns>Неуспешный результат с новым типом данных.</returns>
    public Result<TResult> SameFailure<TResult>() => Result<TResult>.Failure(ErrorMessage!, ErrorType);

    /// <summary>
    /// Создает неуспешный результат без данных с теми же ошибками.
    /// </summary>
    /// <returns>Неуспешный результат без данных.</returns>
    public Result SameFailure() => Result.Failure(ErrorMessage!, ErrorType);
}

/// <summary>
/// Класс для представления результата операции без данных.
/// </summary>
public class Result(bool isSuccess, string? errorMessage, ErrorType errorType = default)
{
    /// <summary>
    /// Флаг успешности операции.
    /// </summary>
    public bool IsSuccess { get; } = isSuccess;
    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string? ErrorMessage { get; } = errorMessage;
    /// <summary>
    /// Тип ошибки.
    /// </summary>
    public ErrorType ErrorType { get; } = errorType;

    /// <summary>
    /// Создает успешный результат.
    /// </summary>
    /// <returns>Успешный результат.</returns>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Создает неуспешный результат с ошибкой.
    /// </summary>
    /// <param name="error">Сообщение об ошибке.</param>
    /// <param name="errorType">Тип ошибки.</param>
    /// <returns>Неуспешный результат.</returns>
    public static Result Failure(string error, ErrorType errorType = ErrorType.Exception) =>
        new(false, error, errorType);
}