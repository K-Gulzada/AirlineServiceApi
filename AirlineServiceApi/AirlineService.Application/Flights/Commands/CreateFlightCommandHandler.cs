using AirlineService.Application.Common.Interfaces;
using AirlineService.Application.Flights.DTOs;
using AirlineService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Handler for CreateFlightCommand.
/// </summary>
public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, FlightDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateFlightCommandHandler> _logger;

    public CreateFlightCommandHandler(
        IApplicationDbContext context, 
        ICacheService cacheService,
        ICurrentUserService currentUserService,
        ILogger<CreateFlightCommandHandler> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<FlightDto> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = new Flight
        {
            Origin = request.Origin,
            Destination = request.Destination,
            Departure = request.Departure,
            Arrival = request.Arrival,
            Status = request.Status
        };

        await _context.Flights.AddAsync(flight);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate flights cache
        await _cacheService.RemoveByPatternAsync("flights_*");

        _logger.LogInformation("{FlightID} Flight was created by User: {Username}", 
            flight.Id, _currentUserService.Username);

        return new FlightDto
        {
            Id = flight.Id,
            Origin = flight.Origin,
            Destination = flight.Destination,
            Departure = flight.Departure,
            Arrival = flight.Arrival,
            Status = flight.Status
        };
    }
}

