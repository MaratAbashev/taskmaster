using Domain.Abstractions;

namespace Domain.Entities;

public class BoardUser: Entity<long>
{
    public long UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid BoardId { get; set; }
    public TaskBoard? Board { get; set; }

    public bool IsOwner { get; set; }
    public bool CanCreateTasks { get; set; }
}
