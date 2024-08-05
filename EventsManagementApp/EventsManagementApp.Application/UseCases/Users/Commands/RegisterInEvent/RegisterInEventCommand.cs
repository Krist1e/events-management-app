using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;

public record RegisterInEventCommand(string EventId, string UserId) : IRequest;