using Domain.Abstractions;

namespace Domain.Entities;

public class TaskBoard : Entity<Guid>
{
    public string Title { get; set; } = null!;
    public List<BoardUser> BoardUsers { get; set; } = [];
    public List<ToDoTask> BoardTasks { get; set; } = [];

    public TelegramGroup? Group { get; set; }
    public long? GroupId { get; set; }
    public bool HasTelegramGroup { get; set; }
}
