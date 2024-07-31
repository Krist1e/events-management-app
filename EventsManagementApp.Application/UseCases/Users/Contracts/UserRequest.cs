namespace EventsManagementApp.Application.UseCases.Users.Contracts;

public record UserRequest(
    string Id,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);