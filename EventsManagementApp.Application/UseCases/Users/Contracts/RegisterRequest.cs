namespace EventsManagementApp.Application.UseCases.Users.Contracts;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);