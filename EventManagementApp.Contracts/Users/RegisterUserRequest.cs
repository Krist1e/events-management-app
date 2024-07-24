namespace EventManagementApp.Contracts.Users;

public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth
);