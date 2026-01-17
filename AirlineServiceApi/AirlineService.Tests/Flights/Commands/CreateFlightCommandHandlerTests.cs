using AirlineService.Application.Common.Interfaces;
using AirlineService.Application.Flights.Commands;
using AirlineService.Domain.Entities;
using AirlineService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirlineService.Tests.Flights.Commands;

public class CreateFlightCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ILogger<CreateFlightCommandHandler>> _loggerMock;
    private readonly CreateFlightCommandHandler _handler;

    public CreateFlightCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _cacheServiceMock = new Mock<ICacheService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _loggerMock = new Mock<ILogger<CreateFlightCommandHandler>>();

        _currentUserServiceMock.Setup(x => x.Username).Returns("moderator");

        _handler = new CreateFlightCommandHandler(
            _contextMock.Object,
            _cacheServiceMock.Object,
            _currentUserServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_CreatesFlight_AndReturnsFlightDto()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Flight>>();
        _contextMock.Setup(x => x.Flights).Returns(mockSet.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateFlightCommand
        {
            Origin = "Almaty",
            Destination = "Astana",
            Departure = DateTimeOffset.UtcNow,
            Arrival = DateTimeOffset.UtcNow.AddHours(2),
            Status = FlightStatus.InTime
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Almaty", result.Origin);
        Assert.Equal("Astana", result.Destination);
        Assert.Equal(FlightStatus.InTime, result.Status);
    }

    [Fact]
    public async Task Handle_InvalidatesCache_AfterCreatingFlight()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Flight>>();
        _contextMock.Setup(x => x.Flights).Returns(mockSet.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateFlightCommand
        {
            Origin = "Almaty",
            Destination = "Astana",
            Departure = DateTimeOffset.UtcNow,
            Arrival = DateTimeOffset.UtcNow.AddHours(2),
            Status = FlightStatus.InTime
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.RemoveByPatternAsync("flights_*"), Times.Once);
    }

    [Fact]
    public async Task Handle_SavesFlightToDatabase()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Flight>>();
        _contextMock.Setup(x => x.Flights).Returns(mockSet.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateFlightCommand
        {
            Origin = "London",
            Destination = "Paris",
            Departure = DateTimeOffset.UtcNow,
            Arrival = DateTimeOffset.UtcNow.AddHours(1),
            Status = FlightStatus.Delayed
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        mockSet.Verify(x => x.AddAsync(It.Is<Flight>(f => 
            f.Origin == "London" && 
            f.Destination == "Paris" && 
            f.Status == FlightStatus.Delayed), 
            It.IsAny<CancellationToken>()), Times.Once);
        
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

