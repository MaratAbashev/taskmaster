namespace Domain.Models;

public class TokenDto
{
    public string? RefreshToken { get; set; }
    public string? AccessToken { get; set; }
    public long? UserId { get; set; }
    public bool? IsUsed { get; set; }
    public DateTime? RefreshCreatedAt { get; set; }
    public DateTime? RefreshExpiresAt { get; set; }
}