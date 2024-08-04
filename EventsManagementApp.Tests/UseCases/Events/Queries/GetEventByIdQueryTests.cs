using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Events.Queries;

public class GetEventByIdQueryTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly GetEventByIdQueryHandler _handler;
    private readonly Mock<IMapper> _mapperMock;

    public GetEventByIdQueryTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
        Mock<ILogger<GetEventByIdQueryHandler>> loggerMock = new();
        _mapperMock = new Mock<IMapper>();
        Mock<IValidator<GetEventByIdQuery>> validatorMock = new();

        _handler = new GetEventByIdQueryHandler(
            _eventRepositoryMock.Object,
            loggerMock.Object,
            _mapperMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnEvent_WhenEventExists()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var query = new GetEventByIdQuery(eventId.ToString());

        _eventRepositoryMock.Setup(r =>
                r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Event());
        
        _mapperMock.Setup(m => m.Map<EventResponse>(It.IsAny<Event>()))
            .Returns(new EventResponse(
                eventId.ToString(),
                "Event Title",
                "Event Description",
                DateTime.Now,
                DateTime.Now.AddDays(1),
                "Event Location",
                "Event Category",
                100,
                new List<ImageResponse>()
            ));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowEventNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var query = new GetEventByIdQuery(eventId.ToString());

        _eventRepositoryMock.Setup(r =>
                r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Event);

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}