using Domain.Abstractions;

namespace Domain.Entities;

public class ToDoTask : Entity<long>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletingDate { get; set; }

    public Guid BoardId { get; set; }
    public TaskBoard Board { get; set; } = null!;

    public long? AuthorId { get; set; }
    public User? Author { get; set; }

    public List<TaskWorker>? TaskWorkers { get; set; } = [];
}

