using AirlineService.Application.Common.Interfaces;
using AirlineService.Application.Flights.Queries;
using AirlineService.Domain.Entities;
using AirlineService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AirlineService.Tests.Flights.Queries;

public class GetFlightsQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetFlightsQueryHandler _handler;

    public GetFlightsQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetFlightsQueryHandler(_contextMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsFlightsFromCache_WhenCacheExists()
    {
        // Arrange
        var query = new GetFlightsQuery { PageNumber = 1, PageSize = 10 };
        var cachedResult = new Application.Common.Models.PaginatedList<Application.Flights.DTOs.FlightDto>(
            new List<Application.Flights.DTOs.FlightDto>
            {
                new() { Id = 1, Origin = "Almaty", Destination = "Astana", Status = FlightStatus.InTime }
            }, 1, 1, 10);

        _cacheServiceMock
            .Setup(x => x.GetAsync<Application.Common.Models.PaginatedList<Application.Flights.DTOs.FlightDto>>(It.IsAny<string>()))
            .ReturnsAsync(cachedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("Almaty", result.Items[0].Origin);
        _contextMock.Verify(x => x.Flights, Times.Never);
    }

    [Fact]
    public async Task Handle_CallsCache_WithCorrectKey()
    {
        // Arrange
        var cachedResult = new Application.Common.Models.PaginatedList<Application.Flights.DTOs.FlightDto>(
            new List<Application.Flights.DTOs.FlightDto>(), 0, 1, 10);

        _cacheServiceMock
            .Setup(x => x.GetAsync<Application.Common.Models.PaginatedList<Application.Flights.DTOs.FlightDto>>(It.IsAny<string>()))
            .ReturnsAsync(cachedResult);

        var query = new GetFlightsQuery { Origin = "Almaty", Destination = "Astana", PageNumber = 1, PageSize = 10 };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert - cache key is lowercase
        _cacheServiceMock.Verify(x => x.GetAsync<Application.Common.Models.PaginatedList<Application.Flights.DTOs.FlightDto>>(
            It.Is<string>(s => s.Contains("almaty") && s.Contains("astana"))), Times.Once);
    }
}

