using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Events.Commands;

public class CreateEventCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateEventCommandHandler _handler;

    public CreateEventCommandTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        Mock<ILogger<CreateEventCommandHandler>> loggerMock = new();
        _mapperMock = new Mock<IMapper>();
        Mock<IValidator<CreateEventCommand>> validatorMock = new();

        _handler = new CreateEventCommandHandler(
            _unitOfWorkMock.Object,
            _eventRepositoryMock.Object,
            loggerMock.Object,
            _mapperMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnEventResponse_WhenCreateEventSucceeds()
    {
        // Arrange
        var newEventRequest = new CreateEventRequest(
            "Event Title",
            "Event Description",
            DateTime.Now,
            DateTime.Now.AddDays(1),
            "Event Location",
            "Event Category",
            100
        );
        
        var command = new CreateEventCommand(newEventRequest);

        var eventId = Guid.NewGuid();

        _mapperMock.Setup(m => m.Map<CreateEventResponse>(It.IsAny<Event>()))
            .Returns(new CreateEventResponse(eventId.ToString(), newEventRequest.Name, newEventRequest.Description,
                newEventRequest.StartDate, newEventRequest.EndDate, newEventRequest.Location, newEventRequest.Category,
                newEventRequest.Capacity));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventId.ToString(), result.Id);
        Assert.Equal(newEventRequest.Name, result.Name);
        Assert.Equal(newEventRequest.Description, result.Description);
        Assert.Equal(newEventRequest.StartDate, result.StartDate);
        Assert.Equal(newEventRequest.EndDate, result.EndDate);
        Assert.Equal(newEventRequest.Location, result.Location);
        Assert.Equal(newEventRequest.Category, result.Category);
        Assert.Equal(newEventRequest.Capacity, result.Capacity);

        _eventRepositoryMock.Verify(r => r.CreateAsync(
            It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(
            It.IsAny<CancellationToken>()), Times.Once);
    }
}