namespace JobTracker.Domain.Entities;

public class RefreshToken
{
    public required string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;
}