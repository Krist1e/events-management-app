namespace EventsManagementApp.Application.UseCases.Users.Contracts;

public record UserResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);