using AirlineService.Application.Auth.DTOs;
using MediatR;

namespace AirlineService.Application.Auth.Commands;

/// <summary>
/// Command to authenticate a user and get a JWT token.
/// </summary>
public class LoginCommand : IRequest<TokenResponse?>
{
    /// <summary>
    /// The username for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

