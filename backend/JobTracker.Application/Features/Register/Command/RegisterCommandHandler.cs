using JobTracker.Application.Features.Register.Command;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Exceptions;
using MediatR;

namespace JobTracker.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        // IAuthService hides UserManager from Application layer
        // Handler never imports anything from Infrastructure
        _authService = authService;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (userId, error) = await _authService.RegisterAsync(request.Email, request.Password);

        if (error is not null)
            throw new ValidationException(error);

        return userId;
    }
}