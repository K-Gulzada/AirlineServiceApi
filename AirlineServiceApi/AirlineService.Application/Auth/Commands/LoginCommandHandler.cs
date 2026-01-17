using AirlineService.Application.Auth.DTOs;
using AirlineService.Application.Common.Interfaces;
using MediatR;

namespace AirlineService.Application.Auth.Commands;

/// <summary>
/// Handler for the LoginCommand.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse?>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.ValidateCredentialsAsync(request.Username, request.Password);

        if (user == null)
            return null;

        var token = _authService.GenerateToken(user);

        return new TokenResponse
        {
            AccessToken = token,
            TokenType = "Bearer",
            Username = user.Username,
            Role = user.Role.Code
        };
    }
}
