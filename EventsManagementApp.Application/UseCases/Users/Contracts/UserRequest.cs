namespace EventsManagementApp.Application.UseCases.Users.Contracts;

public record UserRequest(
    long Id,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);