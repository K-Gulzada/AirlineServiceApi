using AirlineService.Application.Common.Interfaces;
using AirlineService.Application.Flights.DTOs;
using AirlineService.Domain.Entities;
using MediatR;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Handler for CreateFlightCommand.
/// </summary>
public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, FlightDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public CreateFlightCommandHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
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

