using AirlineService.Application.Common.Models;
using AirlineService.Application.Flights.DTOs;
using MediatR;

namespace AirlineService.Application.Flights.Queries;

/// <summary>
/// Query to get a paginated list of flights with optional filtering.
/// </summary>
public class GetFlightsQuery : IRequest<PaginatedList<FlightDto>>
{
    /// <summary>
    /// Filter by origin (optional).
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Filter by destination (optional).
    /// </summary>
    public string? Destination { get; set; }

    /// <summary>
    /// Page number (default is 1).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default is 10).
    /// </summary>
    public int PageSize { get; set; } = 10;
}
