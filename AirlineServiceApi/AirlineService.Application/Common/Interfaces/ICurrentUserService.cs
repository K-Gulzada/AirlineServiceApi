namespace AirlineService.Application.Common.Interfaces;

/// <summary>
/// Interface to get current user information.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user's ID.
    /// </summary>
    int? UserId { get; }

    /// <summary>
    /// Gets the current user's username.
    /// </summary>
    string? Username { get; }

    /// <summary>
    /// Gets the current user's role.
    /// </summary>
    string? Role { get; }
}

