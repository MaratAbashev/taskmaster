namespace Domain.Models;

/// <summary>
/// DTO для задачи.
/// </summary>
public class TaskDto
{
    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Название задачи.
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Уровень приоритета задачи.
    /// </summary>
    public PriorityLevel? PriorityLevel { get; set; }
    /// <summary>
    /// Текущий статус задачи.
    /// </summary>
    public TaskStatus? Status { get; set; }
    /// <summary>
    /// Дата создания задачи.
    /// </summary>
    public DateTime? CreationDate { get; set; }
    /// <summary>
    /// Срок выполнения задачи.
    /// </summary>
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// Дата одобрения задачи.
    /// </summary>
    public DateTime? ApprovingDate { get; set; }
    /// <summary>
    /// Дата отправки задачи на проверку.
    /// </summary>
    public DateTime? SentToApproveDate { get; set; }
    /// <summary>
    /// Идентификатор доски задач.
    /// </summary>
    public Guid BoardId { get; set; }
    /// <summary>
    /// Список исполнителей задачи.
    /// </summary>
    public List<UserDto>? TaskWorkers { get; set; }
    /// <summary>
    /// Руководитель задачи.
    /// </summary>
    public UserDto? Leader { get; set; }
    /// <summary>
    /// Идентификатор руководителя задачи.
    /// </summary>
    public long? LeaderId { get; set; }
    /// <summary>
    /// Автор задачи.
    /// </summary>
    public UserDto? Author { get; set; }
    /// <summary>
    /// Идентификатор автора задачи.
    /// </summary>
    public long? AuthorId { get; set; }
}