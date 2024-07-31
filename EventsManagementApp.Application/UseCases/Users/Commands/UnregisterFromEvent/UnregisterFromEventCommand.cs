using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;

public record UnregisterFromEventCommand(string EventId, string UserId) : IRequest<bool>;