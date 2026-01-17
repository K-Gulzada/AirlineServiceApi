using AirlineService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Handler for UpdateFlightStatusCommand.
/// </summary>
public class UpdateFlightStatusCommandHandler : IRequestHandler<UpdateFlightStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UpdateFlightStatusCommandHandler> _logger;

    public UpdateFlightStatusCommandHandler(
        IApplicationDbContext context, 
        ICacheService cacheService,
        ICurrentUserService currentUserService,
        ILogger<UpdateFlightStatusCommandHandler> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        var flight = await _context.Flights
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (flight == null)
        {
            _logger.LogWarning("{FlightId} FlightID not found | User: {Username}",
                request.Id, _currentUserService.Username);
            return false;
        }

        var oldStatus = flight.Status;
        flight.Status = request.Status;
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate flights cache
        await _cacheService.RemoveByPatternAsync("flights_*");

        _logger.LogInformation("{FlightId} Flight status updated by User: {Username}",
            flight.Id, _currentUserService.Username);

        return true;
    }
}

