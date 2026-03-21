namespace JobTracker.Application.Features.Auth.Commands.Login;

public class AuthResponseDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}