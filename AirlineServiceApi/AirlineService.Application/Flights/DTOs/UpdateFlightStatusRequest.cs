using AirlineService.Domain.Enums;

namespace AirlineService.Application.Flights.DTOs;

/// <summary>
/// Request model for updating a flight's status.
/// </summary>
public class UpdateFlightStatusRequest
{
    /// <summary>
    /// New flight status (0 = InTime, 1 = Delayed, 2 = Cancelled).
    /// </summary>
    /// <example>1</example>
    public FlightStatus Status { get; set; }
}

