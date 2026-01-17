using AirlineService.Application.Common;
using AirlineService.Application.Common.Interfaces;
using AirlineService.Application.Common.Models;
using AirlineService.Application.Flights.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Application.Flights.Queries;

public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, PaginatedList<FlightDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public GetFlightsQueryHandler(
        IApplicationDbContext context,
        ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<PaginatedList<FlightDto>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = GenerateCacheKey(request.Origin, request.Destination, request.PageNumber, request.PageSize);
        
        var cachedResult = await _cacheService.GetAsync<PaginatedList<FlightDto>>(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult;
        }

        var query = _context.Flights.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Origin))
        {
            query = query.Where(f => f.Origin.ToLower().Contains(request.Origin.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(request.Destination))
        {
            query = query.Where(f => f.Destination.ToLower().Contains(request.Destination.ToLower()));
        }

        query = query.OrderBy(f => f.Arrival);

        var totalCount = await query.CountAsync(cancellationToken);

        var flights = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(f => new FlightDto
            {
                Id = f.Id,
                Origin = f.Origin,
                Destination = f.Destination,
                Departure = f.Departure,
                Arrival = f.Arrival,
                Status = f.Status
            })
            .ToListAsync(cancellationToken);

        var result = new PaginatedList<FlightDto>(flights, totalCount, request.PageNumber, request.PageSize);

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(Constants.Cache.DefaultExpirationMinutes));

        return result;
    }

    private static string GenerateCacheKey(string? origin, string? destination, int pageNumber, int pageSize)
    {
        var originPart = string.IsNullOrWhiteSpace(origin) ? "all" : origin.ToLower().Trim();
        var destPart = string.IsNullOrWhiteSpace(destination) ? "all" : destination.ToLower().Trim();
        return $"flights:{originPart}:{destPart}:page{pageNumber}:size{pageSize}";
    }
}
