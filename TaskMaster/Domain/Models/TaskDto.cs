namespace Domain.Models;

public class TaskDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletingDate { get; set; }
    public Guid BoardId { get; set; }
    public List<UserDto>? TaskWorkers { get; set; }
    public UserDto? Author { get; set; }
}