using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Users.Commands;

public class RegisterInEventCommandTests
{
    private readonly RegisterInEventCommandHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public RegisterInEventCommandTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<ILogger<RegisterInEventCommandHandler>> loggerMock = new();
        Mock<IValidator<RegisterInEventCommand>> validatorMock = new();
        
        _handler = new RegisterInEventCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object,
            validatorMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ShouldRegisterUserInEvent_WhenUserExistsAndEventExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var command = new RegisterInEventCommand(eventId.ToString(), userId.ToString());
        
        _userRepositoryMock.Setup(r => r.AddUserToEventAsync(userId, eventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        _userRepositoryMock.Verify(r => r.AddUserToEventAsync(userId, eventId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var command = new RegisterInEventCommand(userId.ToString(), eventId.ToString());
        
        _userRepositoryMock.Setup(r => r.AddUserToEventAsync(userId, eventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<RegisterInEventFailedException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}