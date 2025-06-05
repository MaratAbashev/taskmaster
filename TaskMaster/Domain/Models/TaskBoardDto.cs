namespace Domain.Models;

public class TaskBoardDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public List<UserDto>? Users { get; set; }
    public List<TaskDto>? Tasks { get; set; }
    public string? TelegramGroupLink { get; set; }
}