using MediatR;

namespace EventsManagementApp.Application.UseCases.Auth.Logout;

public record LogoutCommand() : IRequest;