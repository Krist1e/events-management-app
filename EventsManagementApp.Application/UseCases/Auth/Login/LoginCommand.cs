using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Auth.Login;

public record LoginCommand(UserRequest User) : IRequest<bool>;