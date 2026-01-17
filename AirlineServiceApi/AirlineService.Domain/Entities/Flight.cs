using AirlineService.Domain.Enums;

namespace AirlineService.Domain.Entities;

/// <summary>
/// Represents a flight with origin, destination, and schedule information.
/// </summary>
public class Flight
{
    /// <summary>
    /// Unique identifier for the flight.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Origin airport/city (max 256 characters).
    /// </summary>
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    /// Destination airport/city (max 256 characters).
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Departure time in the timezone of the origin location.
    /// </summary>
    public DateTimeOffset Departure { get; set; }

    /// <summary>
    /// Arrival time in the timezone of the destination location.
    /// </summary>
    public DateTimeOffset Arrival { get; set; }

    /// <summary>
    /// Current status of the flight.
    /// </summary>
    public FlightStatus Status { get; set; }
}

