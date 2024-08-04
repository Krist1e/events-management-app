using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Users.Commands;

public class UnregisterFromEventCommandTests
{
    private readonly UnregisterFromEventCommandHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UnregisterFromEventCommandTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<ILogger<UnregisterFromEventCommandHandler>> loggerMock = new();
        Mock<IValidator<UnregisterFromEventCommand>> validatorMock = new();

        _handler = new UnregisterFromEventCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUnregisterUserFromEvent_WhenUserIsRegistered()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var command = new UnregisterFromEventCommand(eventId.ToString(), userId.ToString());

        _userRepositoryMock.Setup(r => r.RemoveUserFromEventAsync(userId, eventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(r => r.RemoveUserFromEventAsync(userId, eventId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowUnregisterFromEventFailedException_WhenUserIsNotRegistered()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var command = new UnregisterFromEventCommand(eventId.ToString(), userId.ToString());

        _userRepositoryMock.Setup(r => r.RemoveUserFromEventAsync(userId, eventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnregisterFromEventFailedException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}