namespace EventsManagementApp.Application.UseCases.Auth.Contracts;

public record LoginRequest(
    string Email,
    string Password);