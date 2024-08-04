using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Users.Queries;

public class GetUserByIdQueryHandlerTests
{
    private readonly GetUserByIdQueryHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        Mock<ILogger<GetUserByIdQueryHandler>> loggerMock = new();
        Mock<IValidator<GetUserByIdQuery>> validatorMock = new();

        _handler = new GetUserByIdQueryHandler(
            _userRepositoryMock.Object,
            loggerMock.Object,
            _mapperMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnUserResponse_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery(userId.ToString());


        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User());

        _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>()))
            .Returns(new UserResponse(
                userId.ToString(),
                "test@gmail.com",
                "Test User",
                "Test User",
                DateOnly.FromDateTime(DateTime.Now)
            ));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery(userId.ToString());

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}