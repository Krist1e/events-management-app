namespace EventsManagementApp.Application.UseCases.Auth.Contracts;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);