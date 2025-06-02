using Domain.Abstractions;

namespace Domain.Entities;

public class User : Entity<long>
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoUrl { get; set; }

    public string NameToPing
    {
        get
        {
            var nameToPing = string.IsNullOrEmpty(FirstName) 
                ? "Пользователь"
                : string.IsNullOrEmpty(LastName) 
                    ? FirstName 
                    : $"{FirstName} {LastName}";
            return !string.IsNullOrEmpty(Username)
                ? $"@{Username}"
                : $"[{nameToPing}](tg://user?id={Id})";
        }
    }

    public List<BoardUser>? BoardUsers { get; set; } = [];
    public List<TaskWorker>? TaskWorkers { get; set; } = [];
    public List<ToDoTask>? AuthoredTasks { get; set; } = [];
}
