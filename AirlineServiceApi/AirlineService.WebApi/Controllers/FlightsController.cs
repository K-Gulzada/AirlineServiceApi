using AirlineService.Application.Common.Models;
using AirlineService.Application.Flights.DTOs;
using AirlineService.Application.Flights.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineService.WebApi.Controllers;

/// <summary>
/// Controller for managing flights.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class FlightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of flights, optionally filtered by origin and/or destination.
    /// Results are sorted by arrival time.
    /// </summary>
    /// <param name="origin">Filter by origin (optional).</param>
    /// <param name="destination">Filter by destination (optional).</param>
    /// <param name="pageNumber">Page number (default is 1).</param>
    /// <param name="pageSize">Number of items per page (default is 10).</param>
    /// <returns>Paginated list of flights sorted by arrival time.</returns>
    /// <response code="200">Returns the paginated list of flights.</response>
    /// <response code="401">Unauthorized - valid JWT token required.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<FlightDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaginatedList<FlightDto>>> GetFlights(
        [FromQuery] string? origin,
        [FromQuery] string? destination,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetFlightsQuery
        {
            Origin = origin,
            Destination = destination,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
