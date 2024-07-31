using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Auth.Register;

public record RegisterCommand(RegisterRequest User) : IRequest;