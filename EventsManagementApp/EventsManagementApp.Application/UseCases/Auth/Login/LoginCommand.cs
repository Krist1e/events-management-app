using EventsManagementApp.Application.UseCases.Auth.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Auth.Login;

public record LoginCommand(LoginRequest User) : IRequest;