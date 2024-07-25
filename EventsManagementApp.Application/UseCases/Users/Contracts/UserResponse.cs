namespace EventsManagementApp.Application.UseCases.Users.Contracts;

public record UserResponse(
    long Id,
    string Email,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);