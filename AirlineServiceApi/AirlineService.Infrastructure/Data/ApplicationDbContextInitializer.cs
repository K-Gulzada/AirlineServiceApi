using AirlineService.Application.Common.Interfaces;
using AirlineService.Domain.Entities;
using AirlineService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineService.Infrastructure.Data;

/// <summary>
/// Initializes the database with migrations and seed data.
/// </summary>
public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ApplicationDbContextInitializer> _logger;

    public ApplicationDbContextInitializer(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<ApplicationDbContextInitializer> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <summary>
    /// Applies pending migrations to the database.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedFlightsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync())
            return;

        var roles = new List<Role>
        {
            new Role { Code = "Moderator" }
        };

        _context.Roles.AddRange(roles);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded role: Moderator");
    }

    private async Task SeedUsersAsync()
    {
        if (await _context.Users.AnyAsync())
            return;

        var moderatorRole = await _context.Roles.FirstAsync(r => r.Code == "Moderator");

        var users = new List<User>
        {
            new User
            {
                Username = "moderator",
                Password = _passwordHasher.HashPassword("moderator123"),
                RoleId = moderatorRole.Id
            }
        };

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded user: moderator (Moderator)");
    }

    private async Task SeedFlightsAsync()
    {
        if (await _context.Flights.AnyAsync())
            return;

        var flights = new List<Flight>
        {
            new Flight
            {
                Origin = "Almaty",
                Destination = "Astana",
                Departure = new DateTimeOffset(2026, 1, 20, 8, 0, 0, TimeSpan.FromHours(6)),
                Arrival = new DateTimeOffset(2026, 1, 20, 9, 30, 0, TimeSpan.FromHours(6)),
                Status = FlightStatus.InTime
            },
            new Flight
            {
                Origin = "Astana",
                Destination = "London",
                Departure = new DateTimeOffset(2026, 1, 20, 14, 0, 0, TimeSpan.FromHours(6)),
                Arrival = new DateTimeOffset(2026, 1, 20, 17, 0, 0, TimeSpan.Zero),
                Status = FlightStatus.InTime
            },
            new Flight
            {
                Origin = "London",
                Destination = "New York",
                Departure = new DateTimeOffset(2026, 1, 21, 10, 0, 0, TimeSpan.Zero),
                Arrival = new DateTimeOffset(2026, 1, 21, 13, 0, 0, TimeSpan.FromHours(-5)),
                Status = FlightStatus.Delayed
            },
            new Flight
            {
                Origin = "Almaty",
                Destination = "Dubai",
                Departure = new DateTimeOffset(2026, 1, 22, 6, 0, 0, TimeSpan.FromHours(6)),
                Arrival = new DateTimeOffset(2026, 1, 22, 8, 0, 0, TimeSpan.FromHours(4)),
                Status = FlightStatus.InTime
            },
            new Flight
            {
                Origin = "Dubai",
                Destination = "Astana",
                Departure = new DateTimeOffset(2026, 1, 23, 12, 0, 0, TimeSpan.FromHours(4)),
                Arrival = new DateTimeOffset(2026, 1, 23, 18, 0, 0, TimeSpan.FromHours(6)),
                Status = FlightStatus.Cancelled
            }
        };

        _context.Flights.AddRange(flights);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} sample flights", flights.Count);
    }
}

