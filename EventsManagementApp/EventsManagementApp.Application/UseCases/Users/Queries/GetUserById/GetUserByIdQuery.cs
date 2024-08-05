using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;

public record GetUserByIdQuery(string UserId) : IRequest<UserResponse>;