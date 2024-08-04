using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Events.Commands;

public class UpdateEventCommandTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateEventCommandHandler _handler;
    private readonly Mock<IMapper> _mapperMock;

    public UpdateEventCommandTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<ILogger<UpdateEventCommandHandler>> loggerMock = new();
        Mock<IValidator<UpdateEventCommand>> validatorMock = new();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateEventCommandHandler(
            _eventRepositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object,
            _mapperMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateEvent_WhenEventExists()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventRequest = new EventRequest(
            eventId.ToString(),
            "New Event Name",
            "New Event Description",
            DateTime.Now,
            DateTime.Now.AddDays(1),
            "New Event Location",
            "New Event Category",
            100
        );
        var command = new UpdateEventCommand(eventRequest);

        _mapperMock.Setup(m => m.Map<Event>(It.IsAny<EventRequest>()))
            .Returns(new Event());
        
        _eventRepositoryMock.Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Event()); 
        
        _eventRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _eventRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowEventNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventRequest = new EventRequest(
            eventId.ToString(),
            "New Event Name",
            "New Event Description",
            DateTime.Now,
            DateTime.Now.AddDays(1),
            "New Event Location",
            "New Event Category",
            100
        );
        var command = new UpdateEventCommand(eventRequest);

        _mapperMock.Setup(m => m.Map<Event>(It.IsAny<EventRequest>()))
            .Returns(new Event());
        
        _eventRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<EventNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}