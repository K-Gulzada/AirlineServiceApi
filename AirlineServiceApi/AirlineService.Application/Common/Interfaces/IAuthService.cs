using AirlineService.Domain.Entities;

namespace AirlineService.Application.Common.Interfaces;

/// <summary>
/// Interface for authentication services.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user to generate the token for.</param>
    /// <returns>JWT token string.</returns>
    string GenerateToken(User user);

    /// <summary>
    /// Validates user credentials and returns the user if valid.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The plain text password.</param>
    /// <returns>The user if credentials are valid, null otherwise.</returns>
    Task<User?> ValidateCredentialsAsync(string username, string password);
}

