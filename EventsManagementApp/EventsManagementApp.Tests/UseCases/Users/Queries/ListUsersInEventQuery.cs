using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;
using FluentValidation;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Users.Queries;

public class ListUsersInEventQueryHandlerTests
{
    private readonly ListUsersInEventQueryHandler _handler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<ListUsersInEventQuery>> _validatorMock;

    public ListUsersInEventQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<ListUsersInEventQuery>>();

        _handler = new ListUsersInEventQueryHandler(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnUserResponses_WhenListUsersInEventSucceeds()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var query = new ListUsersInEventQuery(eventId.ToString());

        _userRepositoryMock.Setup(r => r.GetUsersByEventIdAsync(eventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        _mapperMock.Setup(m => m.Map<IEnumerable<UserResponse>>(It.IsAny<IEnumerable<User>>()))
            .Returns(new List<UserResponse>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}