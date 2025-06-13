using Domain.Models;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Application.Models;

/// <summary>
/// Модель ответа с информацией о задаче.
/// </summary>
public class TaskResponse
{
    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Текущий статус задачи.
    /// </summary>
    public TaskStatus Status { get; set; }

    /// <summary>
    /// Срок выполнения задачи.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Дата утверждения задачи.
    /// </summary>
    public DateTime? ApprovingDate { get; set; }

    /// <summary>
    /// Дата отправки задачи на утверждение.
    /// </summary>
    public DateTime? SentToApproveDate { get; set; }

    /// <summary>
    /// Уровень приоритета задачи.
    /// </summary>
    public PriorityLevel PriorityLevel { get; set; }

    /// <summary>
    /// Информация об авторе задачи.
    /// </summary>
    public TaskUserResponse? Author { get; set; }

    /// <summary>
    /// Информация о руководителе задачи.
    /// </summary>
    public TaskUserResponse? Leader { get; set; }

    /// <summary>
    /// Список исполнителей задачи.
    /// </summary>
    public List<TaskUserResponse>? Workers { get; set; }
}
