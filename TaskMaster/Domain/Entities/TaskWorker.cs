using Domain.Abstractions;

namespace Domain.Entities;

public class TaskWorker: Entity<long>
{
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    public long TaskId { get; set; }
    public ToDoTask Task { get; set; } = null!;

    public DateTime? StartedAt { get; set; }
    public bool IsConfirmed { get; set; }
}
