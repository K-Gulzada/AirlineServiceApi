namespace AirlineService.Application.Common.Interfaces;

/// <summary>
/// Interface for password hashing operations.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>The hashed password.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a password against its hash.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <param name="hashedPassword">The hashed password to verify against.</param>
    /// <returns>True if the password matches, false otherwise.</returns>
    bool VerifyPassword(string password, string hashedPassword);
}

