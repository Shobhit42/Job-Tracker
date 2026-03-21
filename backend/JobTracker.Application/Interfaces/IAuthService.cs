namespace JobTracker.Application.Interfaces;

public interface IAuthService
{
    Task<(Guid UserId, string? Error)> RegisterAsync(string email, string password);
    Task<Guid?> VerifyCredentialsAsync(string email, string password);
}