using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Contracts;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;
using FluentValidation;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Events.Queries;

public class ListEventsByUserIdQueryTests
{
    private readonly ListEventsByUserIdQueryHandler _handler;
    private readonly Mock<IMapper> _mapperMock;

    public ListEventsByUserIdQueryTests()
    {
        Mock<IEventRepository> eventRepositoryMock = new();
        _mapperMock = new Mock<IMapper>();
        Mock<IValidator<ListEventsByUserIdQuery>> validatorMock = new();
        
        _handler = new ListEventsByUserIdQueryHandler(
            eventRepositoryMock.Object,
            _mapperMock.Object,
            validatorMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ShouldReturnPagedResponseOfEventResponse_WhenListEventsByUserIdSucceeds()
    {
        // Arrange
        var query = new ListEventsByUserIdQuery(Guid.NewGuid().ToString(), new QueryParameters());
        
        _mapperMock.Setup(m => m.Map<PagedResponse<EventResponse>>(It.IsAny<PagedResponse<Event>>()))
            .Returns(new PagedResponse<EventResponse>(
                new List<EventResponse>(),
                new PaginationMetadata(1, 1, 1)
            ));
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
    }
}