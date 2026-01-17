using AirlineService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Application.Common.Interfaces;

/// <summary>
/// Interface for the application database context.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets the flights table.
    /// </summary>
    DbSet<Flight> Flights { get; }

    /// <summary>
    /// Gets the users table.
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Gets the roles table.
    /// </summary>
    DbSet<Role> Roles { get; }

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

