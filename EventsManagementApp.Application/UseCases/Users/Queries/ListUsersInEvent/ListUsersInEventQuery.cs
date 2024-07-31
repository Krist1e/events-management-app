using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;

public record ListUsersInEventQuery(string EventId) : IRequest<IEnumerable<UserResponse>>;