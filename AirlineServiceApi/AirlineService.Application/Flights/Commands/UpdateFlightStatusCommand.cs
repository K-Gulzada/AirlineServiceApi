using AirlineService.Domain.Enums;
using MediatR;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Command to update a flight's status.
/// </summary>
public class UpdateFlightStatusCommand : IRequest<bool>
{
    /// <summary>
    /// The ID of the flight to update.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The new flight status.
    /// </summary>
    public FlightStatus Status { get; set; }
}

