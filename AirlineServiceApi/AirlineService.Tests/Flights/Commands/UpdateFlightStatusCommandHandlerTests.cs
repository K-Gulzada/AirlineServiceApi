using AirlineService.Application.Common.Interfaces;
using AirlineService.Application.Flights.Commands;
using AirlineService.Domain.Entities;
using AirlineService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirlineService.Tests.Flights.Commands;

public class UpdateFlightStatusCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ILogger<UpdateFlightStatusCommandHandler>> _loggerMock;
    private readonly UpdateFlightStatusCommandHandler _handler;

    public UpdateFlightStatusCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _cacheServiceMock = new Mock<ICacheService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _loggerMock = new Mock<ILogger<UpdateFlightStatusCommandHandler>>();

        _currentUserServiceMock.Setup(x => x.Username).Returns("moderator");

        _handler = new UpdateFlightStatusCommandHandler(
            _contextMock.Object,
            _cacheServiceMock.Object,
            _currentUserServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenFlightExists()
    {
        // Arrange
        var flight = new Flight
        {
            Id = 1,
            Origin = "Almaty",
            Destination = "Astana",
            Status = FlightStatus.InTime
        };

        var mockSet = MockDbSetHelper.CreateMockDbSet(new List<Flight> { flight });
        _contextMock.Setup(x => x.Flights).Returns(mockSet.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new UpdateFlightStatusCommand { Id = 1, Status = FlightStatus.Delayed };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(FlightStatus.Delayed, flight.Status);
    }

    [Fact]
    public async Task Handle_ReturnsFalse_WhenFlightNotFound()
    {
        // Arrange
        var mockSet = MockDbSetHelper.CreateMockDbSet(new List<Flight>());
        _contextMock.Setup(x => x.Flights).Returns(mockSet.Object);

        var command = new UpdateFlightStatusCommand { Id = 999, Status = FlightStatus.Cancelled };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_InvalidatesCache_AfterUpdatingStatus()
    {
        // Arrange
        var flight = new Flight
        {
            Id = 1,
            Origin = "Almaty",
            Destination = "Astana",
            Status = FlightStatus.InTime
        };

        var mockSet = MockDbSetHelper.CreateMockDbSet(new List<Flight> { flight });
        _contextMock.Setup(x => x.Flights).Returns(mockSet.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new UpdateFlightStatusCommand { Id = 1, Status = FlightStatus.Cancelled };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.RemoveByPatternAsync("flights_*"), Times.Once);
    }
}

