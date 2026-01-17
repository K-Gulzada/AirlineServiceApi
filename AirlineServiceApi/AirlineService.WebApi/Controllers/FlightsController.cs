using AirlineService.Application.Common;
using AirlineService.Application.Common.Models;
using AirlineService.Application.Flights.Commands;
using AirlineService.Application.Flights.DTOs;
using AirlineService.Application.Flights.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineService.WebApi.Controllers;

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

    /// <summary>
    /// Creates a new flight. Requires Moderator role.
    /// </summary>
    /// <param name="request">Flight data.</param>
    /// <returns>The created flight.</returns>
    /// <response code="201">Flight created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">Unauthorized - valid JWT token required.</response>
    /// <response code="403">Forbidden - Moderator role required.</response>
    [HttpPost]
    [Authorize(Policy = Constants.Policies.ModeratorOnly)]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<FlightDto>> CreateFlight([FromBody] CreateFlightRequest request)
    {
        var command = new CreateFlightCommand
        {
            Origin = request.Origin,
            Destination = request.Destination,
            Departure = request.Departure,
            Arrival = request.Arrival,
            Status = request.Status
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetFlights), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates the status of an existing flight. Requires Moderator role.
    /// </summary>
    /// <param name="id">The flight ID.</param>
    /// <param name="request">The new status.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Status updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">Unauthorized - valid JWT token required.</response>
    /// <response code="403">Forbidden - Moderator role required.</response>
    /// <response code="404">Flight not found.</response>
    [HttpPut("{id}/status")]
    [Authorize(Policy = Constants.Policies.ModeratorOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFlightStatus(int id, [FromBody] UpdateFlightStatusRequest request)
    {
        var command = new UpdateFlightStatusCommand
        {
            Id = id,
            Status = request.Status
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound(new { message = $"Flight with ID {id} not found." });

        return NoContent();
    }
}
