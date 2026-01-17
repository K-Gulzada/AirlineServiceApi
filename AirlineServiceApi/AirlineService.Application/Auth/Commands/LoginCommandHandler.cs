using AirlineService.Application.Auth.DTOs;
using AirlineService.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AirlineService.Application.Auth.Commands;

/// <summary>
/// Handler for the LoginCommand.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse?>
{
    private readonly IAuthService _authService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IAuthService authService, ILogger<LoginCommandHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<TokenResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.ValidateCredentialsAsync(request.Username, request.Password);

        if (user == null)
        {
            _logger.LogWarning("Login failed for User: {Username}. Reason: Invalid credentials", request.Username);
            return null;
        }

        var token = _authService.GenerateToken(user);

        _logger.LogInformation("Login successful | User: {Username} | Role: {Role}", user.Username, user.Role.Code);

        return new TokenResponse
        {
            AccessToken = token,
            TokenType = "Bearer",
            Username = user.Username,
            Role = user.Role.Code
        };
    }
}
