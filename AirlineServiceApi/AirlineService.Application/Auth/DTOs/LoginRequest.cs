namespace AirlineService.Application.Auth.DTOs;

/// <summary>
/// Request model for user login.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The username for authentication.
    /// </summary>
    /// <example>moderator</example>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for authentication.
    /// </summary>
    /// <example>moderator123</example>
    public string Password { get; set; } = string.Empty;
}
