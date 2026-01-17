using AirlineService.Application.Flights.DTOs;
using AirlineService.Domain.Enums;
using MediatR;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Command to create a new flight.
/// </summary>
public class CreateFlightCommand : IRequest<FlightDto>
{
    /// <summary>
    /// Origin airport/city.
    /// </summary>
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    /// Destination airport/city.
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Departure time.
    /// </summary>
    public DateTimeOffset Departure { get; set; }

    /// <summary>
    /// Arrival time.
    /// </summary>
    public DateTimeOffset Arrival { get; set; }

    /// <summary>
    /// Initial flight status.
    /// </summary>
    public FlightStatus Status { get; set; }
}

