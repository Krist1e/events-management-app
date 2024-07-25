using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public Task<EventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}