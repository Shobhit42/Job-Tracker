using MediatR;

namespace JobTracker.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}