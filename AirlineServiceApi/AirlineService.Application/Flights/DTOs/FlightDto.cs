using AirlineService.Domain.Enums;

namespace AirlineService.Application.Flights.DTOs;

/// <summary>
/// Data transfer object for Flight.
/// </summary>
public class FlightDto
{
    /// <summary>
    /// Unique identifier for the flight.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Origin airport/city.
    /// </summary>
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    /// Destination airport/city.
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
    /// Current status of the flight: InTime, Delayed, or Cancelled.
    /// </summary>
    public FlightStatus Status { get; set; }

    /// <summary>
    /// String representation of the flight status.
    /// </summary>
    public string StatusName => Status.ToString();
}

