using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Constants;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Auth.Contracts;
using EventsManagementApp.Application.UseCases.Auth.Register;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Auth;

public class RegisterCommandTests
{
    private readonly RegisterCommandHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;

    public RegisterCommandTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<ILogger<RegisterCommandHandler>> loggerMock = new();
        _mapperMock = new Mock<IMapper>();

        _handler = new RegisterCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateUserAndAssignRole_WhenRegistrationSucceeds()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "test@gmail.com",
            "Password123",
            "Test",
            "User",
            DateOnly.FromDateTime(DateTime.Now)
        );
        var command = new RegisterCommand(registerRequest);
        var user = new User { Id = Guid.NewGuid(), Email = registerRequest.Email };
        
        _mapperMock.Setup(m => m.Map<User>(registerRequest))
            .Returns(user);
        _userRepositoryMock.Setup(r => r.CreateAsync(user, registerRequest.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.AddRoleToUserAsync(user.Id, Roles.User, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(r => r.CreateAsync(user, registerRequest.Password, It.IsAny<CancellationToken>()),
            Times.Once);
        _userRepositoryMock.Verify(r => r.AddRoleToUserAsync(user.Id, Roles.User, It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowRegisterFailedException_WhenUserCreationFails()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "test@gmail.com",
            "Password123",
            "Test",
            "User",
            DateOnly.FromDateTime(DateTime.Now)
        );
        var command = new RegisterCommand(registerRequest);
        var user = new User { Id = Guid.NewGuid(), Email = registerRequest.Email };

        _mapperMock.Setup(m => m.Map<User>(registerRequest))
            .Returns(user);
        _userRepositoryMock.Setup(r => r.CreateAsync(user, registerRequest.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<RegisterFailedException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowRoleAssignmentFailedException_WhenRoleAssignmentFails()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "test@gmail.com",
            "Password123",
            "Test",
            "User",
            DateOnly.FromDateTime(DateTime.Now)
        );
        var command = new RegisterCommand(registerRequest);
        var user = new User { Id = Guid.NewGuid(), Email = registerRequest.Email };

        _mapperMock.Setup(m => m.Map<User>(registerRequest))
            .Returns(user);

        _userRepositoryMock.Setup(r => r.CreateAsync(user, registerRequest.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.AddRoleToUserAsync(user.Id, Roles.User, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<RoleAssignmentFailedException>(() =>
            _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}