using Domain.Abstractions;

namespace Domain.Entities;

public class TelegramGroup: Entity<long>
{
    public string? Link { get; set; }
    public TaskBoard? Board { get; set; }
    public Guid BoardId { get; set; }
}