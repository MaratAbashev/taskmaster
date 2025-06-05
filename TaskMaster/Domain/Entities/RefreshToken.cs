using Domain.Abstractions;

namespace Domain.Entities;

public class RefreshToken: Entity<long>
{
    public string Token { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}