using AirlineService.Domain.Enums;

namespace AirlineService.Application.Flights.DTOs;

/// <summary>
/// Request model for creating a new flight.
/// </summary>
public class CreateFlightRequest
{
    /// <summary>
    /// Origin airport/city.
    /// </summary>
    /// <example>Almaty</example>
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    /// Destination airport/city.
    /// </summary>
    /// <example>Astana</example>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Departure time.
    /// </summary>
    /// <example>2026-01-25T08:00:00+06:00</example>
    public DateTimeOffset Departure { get; set; }

    /// <summary>
    /// Arrival time.
    /// </summary>
    /// <example>2026-01-25T09:30:00+06:00</example>
    public DateTimeOffset Arrival { get; set; }

    /// <summary>
    /// Initial flight status (0 = InTime, 1 = Delayed, 2 = Cancelled).
    /// </summary>
    /// <example>0</example>
    public FlightStatus Status { get; set; }
}

