namespace AirlineService.Domain.Enums;

/// <summary>
/// Represents the status of a flight.
/// </summary>
public enum FlightStatus
{
    /// <summary>
    /// Flight is on time.
    /// </summary>
    InTime = 0,

    /// <summary>
    /// Flight is delayed.
    /// </summary>
    Delayed = 1,

    /// <summary>
    /// Flight has been cancelled.
    /// </summary>
    Cancelled = 2
}

