namespace EventsManagementApp.Application.UseCases.Users.Contracts;

public record LoginRequest(
    string Email,
    string Password);