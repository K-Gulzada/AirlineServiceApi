using AirlineService.Application.Auth.Commands;
using AirlineService.Application.Common.Interfaces;
using AirlineService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirlineService.Tests.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _loggerMock = new Mock<ILogger<LoginCommandHandler>>();
        _handler = new LoginCommandHandler(_authServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsToken_WhenCredentialsValid()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "moderator",
            Password = "hashed",
            Role = new Role { Id = 1, Code = "Moderator" }
        };

        _authServiceMock
            .Setup(x => x.ValidateCredentialsAsync("moderator", "moderator123"))
            .ReturnsAsync(user);

        _authServiceMock
            .Setup(x => x.GenerateToken(user))
            .Returns("test-jwt-token");

        var command = new LoginCommand { Username = "moderator", Password = "moderator123" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-jwt-token", result.AccessToken);
        Assert.Equal("moderator", result.Username);
        Assert.Equal("Moderator", result.Role);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenCredentialsInvalid()
    {
        // Arrange
        _authServiceMock
            .Setup(x => x.ValidateCredentialsAsync("invalid", "wrong"))
            .ReturnsAsync((User?)null);

        var command = new LoginCommand { Username = "invalid", Password = "wrong" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_CallsGenerateToken_OnlyWhenUserFound()
    {
        // Arrange
        _authServiceMock
            .Setup(x => x.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var command = new LoginCommand { Username = "test", Password = "test" };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _authServiceMock.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}

