using AirlineService.Application.Flights.Commands;
using AirlineService.Domain.Enums;

namespace AirlineService.Tests.Flights.Commands;

public class CreateFlightCommandValidatorTests
{
    private readonly CreateFlightCommandValidator _validator;

    public CreateFlightCommandValidatorTests()
    {
        _validator = new CreateFlightCommandValidator();
    }

    [Fact]
    public void Validate_ReturnsSuccess_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateFlightCommand
        {
            Origin = "Almaty",
            Destination = "Astana",
            Departure = DateTimeOffset.UtcNow.AddHours(1),
            Arrival = DateTimeOffset.UtcNow.AddHours(3),
            Status = FlightStatus.InTime
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ReturnsError_WhenOriginIsEmpty()
    {
        // Arrange
        var command = new CreateFlightCommand
        {
            Origin = "",
            Destination = "Astana",
            Departure = DateTimeOffset.UtcNow.AddHours(1),
            Arrival = DateTimeOffset.UtcNow.AddHours(3),
            Status = FlightStatus.InTime
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Origin");
    }

    [Fact]
    public void Validate_ReturnsError_WhenDestinationIsEmpty()
    {
        // Arrange
        var command = new CreateFlightCommand
        {
            Origin = "Almaty",
            Destination = "",
            Departure = DateTimeOffset.UtcNow.AddHours(1),
            Arrival = DateTimeOffset.UtcNow.AddHours(3),
            Status = FlightStatus.InTime
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Destination");
    }

    [Fact]
    public void Validate_ReturnsError_WhenArrivalIsBeforeDeparture()
    {
        // Arrange
        var command = new CreateFlightCommand
        {
            Origin = "Almaty",
            Destination = "Astana",
            Departure = DateTimeOffset.UtcNow.AddHours(3),
            Arrival = DateTimeOffset.UtcNow.AddHours(1),
            Status = FlightStatus.InTime
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Arrival");
    }

    [Fact]
    public void Validate_ReturnsError_WhenOriginExceedsMaxLength()
    {
        // Arrange
        var command = new CreateFlightCommand
        {
            Origin = new string('A', 257),
            Destination = "Astana",
            Departure = DateTimeOffset.UtcNow.AddHours(1),
            Arrival = DateTimeOffset.UtcNow.AddHours(3),
            Status = FlightStatus.InTime
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Origin");
    }
}

