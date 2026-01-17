namespace AirlineService.Domain.Entities;

/// <summary>
/// Represents a user role for authorization.
/// </summary>
public class Role
{
    /// <summary>
    /// Unique identifier for the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique role code (e.g., "Moderator"). Max 256 characters.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to users with this role.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}

