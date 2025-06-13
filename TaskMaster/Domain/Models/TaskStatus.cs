namespace Domain.Models;

/// <summary>
/// Статус задачи.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Задача ожидает выполнения.
    /// </summary>
    ToDo = 1,
    /// <summary>
    /// Задача в процессе выполнения.
    /// </summary>
    InProgress = 2,
    /// <summary>
    /// Задача на проверке.
    /// </summary>
    OnReview = 3,
    /// <summary>
    /// Задача одобрена.
    /// </summary>
    Approved = 4,
    /// <summary>
    /// Задача отклонена.
    /// </summary>
    Failed = 5
}
