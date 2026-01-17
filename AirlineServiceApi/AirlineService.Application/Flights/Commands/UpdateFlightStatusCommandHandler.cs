using AirlineService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Handler for UpdateFlightStatusCommand.
/// </summary>
public class UpdateFlightStatusCommandHandler : IRequestHandler<UpdateFlightStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public UpdateFlightStatusCommandHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        var flight = await _context.Flights
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (flight == null)
            return false;

        flight.Status = request.Status;
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate flights cache
        await _cacheService.RemoveByPatternAsync("flights_*");

        return true;
    }
}

