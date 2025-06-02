namespace Domain.Models;

public class UserDto
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
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
    public long? AuthDate { get; set; }
    public string? Hash { get; set; }
}