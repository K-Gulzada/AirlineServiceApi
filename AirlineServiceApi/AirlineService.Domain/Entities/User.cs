namespace AirlineService.Domain.Entities;

/// <summary>
/// Represents a user/client of the airline service.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique username (max 256 characters).
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password (max 256 characters).
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the user's role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Navigation property to the user's role.
    /// </summary>
    public Role Role { get; set; } = null!;
}

