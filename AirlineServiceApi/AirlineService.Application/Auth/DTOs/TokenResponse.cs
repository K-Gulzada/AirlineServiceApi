namespace AirlineService.Application.Auth.DTOs;

/// <summary>
/// Response containing the JWT token after successful authentication.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// The JWT access token.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type (always "Bearer").
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// The authenticated user's username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The authenticated user's role.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
